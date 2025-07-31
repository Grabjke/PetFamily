using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core;
using PetFamily.SharedKernel;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;

namespace PetFamily.Files.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _client;
    private readonly ILogger<MinioProvider> _logger;
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private const int ONE_DAY_EXPIRY = 60 * 60 * 24 * 7;
    private const string BUCKET_NAME = "photos";

    public MinioProvider(IMinioClient client, ILogger<MinioProvider> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphore = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketsNotExistCreateBucket(filesList.Select(file => file.Info.BucketName),
                cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphore, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return Error.Failure("file.upload", "Fail to upload files in minio");

            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);
            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    public async Task<Result<IReadOnlyList<string>, Error>> RemoveFiles(
        IEnumerable<string> filesNames,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesNames.ToList();
        try
        {
            var tasks = filesList.Select(async file =>
                await RemoveObject(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);
            if (pathsResult.Any(p => p.IsFailure))
                return Error.Failure("file.upload", "Fail to upload files in minio");

            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to remove files in minio, files amount: {amount}", filesList.Count);
            return Error.Failure("file.remove", "Fail to remove files in minio");
        }
    }


    public async Task<Result<string, Error>> PresignedGetFile(
        string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileName)
                .WithExpiry(ONE_DAY_EXPIRY);

            var url = await _client.PresignedGetObjectAsync(args);

            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to presigned files in minio, files name: {name}", fileName);
            return Error.Failure("file.presigned", "Fail to presigned files in minio");
        }
    }

    public async Task<UnitResult<Error>> DeleteFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await IfBucketsNotExistCreateBucket([fileInfo.BucketName], cancellationToken);

            var statArgs = new StatObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);

            var objectStat = await _client.StatObjectAsync(statArgs, cancellationToken);
            if (objectStat is null)
                return Result.Success<Error>();

            var removeArgs = new RemoveObjectArgs()
                .WithBucket(BUCKET_NAME)
                .WithObject(fileInfo.FilePath.Path);
            await _client.RemoveObjectAsync(removeArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to remove file in minio with file name {path} in bucket {bucket}",
                fileInfo.FilePath.Path,
                BUCKET_NAME);

            return Error.Failure("file.remove", "Fail to remove file in minio");
        }

        return Result.Success<Error>();
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphore,
        CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.Info.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.Info.FilePath.Path);

            await _client.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.Info.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.Info.FilePath.Path,
                fileData.Info.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task<Result<string, Error>> RemoveObject(
        string fileName,
        SemaphoreSlim semaphore,
        CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(BUCKET_NAME)
            .WithObject(fileName);
        try
        {
            await _client.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to remove file in minio with file name {path} in bucket {bucket}",
                fileName,
                BUCKET_NAME);

            return Error.Failure("file.remove", "Fail to remove file in minio");
        }
    }

    private async Task IfBucketsNotExistCreateBucket(
        IEnumerable<string> buckets,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketNames = [..buckets];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExistResult = await _client.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExistResult == false)
            {
                var buketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _client.MakeBucketAsync(buketArgs, cancellationToken);
            }
        }
    }
}