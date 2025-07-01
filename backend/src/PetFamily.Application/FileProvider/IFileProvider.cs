using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);
    
    Task<Result<IReadOnlyList<string>, Error>> RemoveFiles(
        IEnumerable<string> filesNames,
        CancellationToken cancellationToken = default);
    
    Task<Result<string, Error>> PresignedGetFile(
        string fileName,
        CancellationToken cancellationToken = default);
    
    Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphore,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> RemoveObject(
        string fileName,
        SemaphoreSlim semaphore,
        CancellationToken cancellationToken = default);

    Task IfBucketsNotExistCreateBucket(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);
}