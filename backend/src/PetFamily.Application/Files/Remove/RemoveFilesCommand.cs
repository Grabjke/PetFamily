namespace PetFamily.Application.Files.Remove;

public record RemoveFilesCommand(IEnumerable<string> FilesNames);