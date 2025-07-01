using PetFamily.Application.Dtos;

namespace PetFamily.Application.Files.Upload;

public record UploadFilesCommand(IEnumerable<CreateFileDto> files);