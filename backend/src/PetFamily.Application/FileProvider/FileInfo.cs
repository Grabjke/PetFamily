using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.FileProvider;

public record FileInfo(FilePath FilePath,string BucketName);