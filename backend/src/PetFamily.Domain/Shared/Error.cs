
namespace PetFamily.Domain.Shared;

public class Error
{
    public const string SEPARATOR = "||";
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public string? PropertyName { get; }

    private Error(string code, string message, ErrorType type, string? propertyName = null)
    {
        Code = code;
        Message = message;
        Type = type;
        PropertyName = propertyName;
    }

    public static Error Validation(string code, string message,string? propertyName=null) =>
        new Error(code, message, ErrorType.Validation,propertyName);

    public static Error NotFound(string code, string message) =>
        new Error(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) =>
        new Error(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) =>
        new Error(code, message, ErrorType.Conflict);

    public static Error Duplication(string code, string message) =>
        new Error(code, message, ErrorType.Duplication);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 2)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        return new Error(parts[0], parts[1], type);
    }
   
    public ErrorList ToErrorList()=>new([this]);
}