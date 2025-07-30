using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;

namespace PetFamily.Core;

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

    Task<UnitResult<Error>> DeleteFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);
}