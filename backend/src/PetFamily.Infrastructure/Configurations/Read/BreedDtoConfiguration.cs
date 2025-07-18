﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos.Query;

namespace PetFamily.Infrastructure.Configurations.Read;

public class BreedDtoConfiguration:IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Name)
            .HasColumnName("name");
        
    }
}