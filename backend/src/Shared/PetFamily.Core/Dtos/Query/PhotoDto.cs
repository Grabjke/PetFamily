using System.Text.Json.Serialization;

namespace PetFamily.Core.Dtos.Query;

public class PhotoDto
{
    public FilePathDto PathToStorage { get; init; } = null!;
    public bool IsMain  { get; init; }
}

public class FilePathDto
{
    public string Path { get; init; } = string.Empty;
}