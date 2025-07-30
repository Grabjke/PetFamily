using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Files.Application.Files.Upload;

public record UploadFilesCommand(IEnumerable<CreateFileDto> files):ICommand;