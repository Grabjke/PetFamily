using PetFamily.Core.Abstractions;

namespace PetFamily.Files.Application.Files.Remove;

public record RemoveFilesCommand(IEnumerable<string> FilesNames):ICommand;