﻿using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FileInfo  Info);