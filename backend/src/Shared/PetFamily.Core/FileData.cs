using FileInfo = PetFamily.Core.FileProvider.FileInfo;

namespace PetFamily.Core;

public record FileData(Stream Stream, FileInfo  Info);