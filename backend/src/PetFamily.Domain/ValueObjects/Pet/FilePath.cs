using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record FilePath
{
    //ef
    private FilePath() { }
    
    private static readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".bmp"];

    [JsonConstructor]
    private FilePath(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        if (path == Guid.Empty)
        {
            return Errors.General.ValueIsInvalid(nameof(path));
        }

        if (string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension))
        {
            return Errors.General.ValueIsInvalid(nameof(extension));
        }

        var filePath = path  + extension;

        return new FilePath(filePath);
    }

    public static Result<FilePath, Error> Create(string filePath)
    {
        return new FilePath(filePath);
    }
}