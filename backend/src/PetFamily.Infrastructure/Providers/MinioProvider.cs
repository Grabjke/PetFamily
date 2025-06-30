using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _client;
    private readonly ILogger<MinioProvider> _logger;
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    const int ONE_DAY_EXPIRY = 60 * 60 * 24 * 7;
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
            await IfBucketsNotExistCreateBucket(filesList, cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphore, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

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
                return pathsResult.First().Error;

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
            
            var url= await _client.PresignedGetObjectAsync(args);
            
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to presigned files in minio, files name: {name}", fileName);
            return Error.Failure("file.presigned", "Fail to presigned files in minio");
        }
    }

    public async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphore,
        CancellationToken cancellationToken = default)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.FilePath.Path);

            await _client.PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphore.Release();
        }
    }

    public async Task<Result<string, Error>> RemoveObject(
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

    public async Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketNames = [..filesData.Select(file => file.BucketName)];

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