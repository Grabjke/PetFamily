using PetFamily.Core.Abstractions;

namespace PetFamily.Files.Application.Files.Presigned;

public record PresignedFileCommand(string FileName) : ICommand;