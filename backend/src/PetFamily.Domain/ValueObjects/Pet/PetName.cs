﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects.Pet;

public record PetName
{
    private PetName(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static Result<PetName, Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Pet name");

        return new PetName(name);
    }
}