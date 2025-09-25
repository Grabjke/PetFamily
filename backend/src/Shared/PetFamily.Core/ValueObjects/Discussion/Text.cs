using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.ValueObjects.Discussion;

public record Text
{
    //ef
    private Text()
    {
    }

    public Text(string text)
    {
        Value = text;
    }

    public string Value { get; }

    public static Result<Text, Error> Create(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Text");

        return new Text(text);
    }
}