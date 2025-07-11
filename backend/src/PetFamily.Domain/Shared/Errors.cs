﻿namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var recordId = id == null ? "": $" for Id '{id}'";
            return Error.Validation("record.not.found", $"record not found {recordId}");
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name ?? "Value";
            return Error.Validation("value.is.required", $"{label} is required");
        }
        
        public static Error InvalidLength(string? name = null, int? maxLength = null, int? minLength = null)
        {
            var label = name ?? "Value";
            var lengthInfo = string.Empty;
    
            if (minLength.HasValue && maxLength.HasValue)
                lengthInfo = $" (expected {minLength}-{maxLength} characters)";
            else if (maxLength.HasValue)
                lengthInfo = $" (max {maxLength} characters)";
            else if (minLength.HasValue)
                lengthInfo = $" (min {minLength} characters)";

            return Error.Validation("invalid.length", $"{label} has invalid length{lengthInfo}");
        }
        
        public static Error AllReadyExist()
        {
            return Error.Validation("record.allready.exist", "record allready exist");
        }
      
        
    }
    public static class Species
    {
        public static Error CannotDeleteBecauseHasAnimals()
        {
            return Error.Validation("species.has.dependencies", 
                "cannot delete species because there are animals associated with it");
        }
    }
    public static class Breed
    {
        public static Error CannotDeleteBecauseHasAnimals()
        {
            return Error.Validation("breed.has.dependencies", 
                "cannot delete breed because there are animals associated with it");
        }
    }
    
    
}