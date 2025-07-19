using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.AggregateRoots;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Species;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Species;
using PetFamily.Domain.ValueObjects.Volunteer;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.App.IntegrationTests;

public static class DatabaseSeeder
{
    public static async Task<Guid> SeedVolunteer(WriteDbContext dbContext)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            FullName.Create("John", "Doe").Value,
            Email.Create("john@doe.com").Value,
            VolunteerDescription.Create("John Doe DESCRIPTION").Value,
            VolunteerExperience.Create(1).Value,
            OwnersPhoneNumber.Create("79920013488").Value);

        await dbContext.Volunteers.AddAsync(volunteer);
        await dbContext.SaveChangesAsync();

        return volunteer.Id;
    }

    public static async Task<(Guid SpeciesId, Guid BreedId)> SeedSpeciesAndBreed(WriteDbContext dbContext)
    {
        var species = PetSpecies.Create(
            SpeciesId.NewSpeciesId(),
            "Кот").Value;

        var breed = Breed.Create(
            BreedId.NewBreedId(),
            "Сиамский").Value;

        await dbContext.Species.AddAsync(species);
        species.AddBreed(breed);
        await dbContext.SaveChangesAsync();

        return (species.Id.Value, breed.Id.Value);
    }

    public static async Task<Guid> SeedPet(
        WriteDbContext dbContext,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        var petSpeciesId = SpeciesId.Create(speciesId);
        var petBreedId = BreedId.Create(breedId);

        var pet = new Pet(
            PetId.NewPetId(),
            PetName.Create("Барсик").Value,
            PetDescription.Create("Умный кот").Value,
            PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
            PetColour.Create("Рыжий").Value,
            PetHealthInformation.Create("Здоровый кот").Value,
            Address.Create("ул. Тестовая", "Москва", "Россия", "123456").Value,
            PetWeight.Create(20).Value,
            PetHeight.Create(30).Value,
            OwnersPhoneNumber.Create("79920013488").Value,
            true,
            Birthday.Create(new DateTime(2010, 1, 1)).Value,
            false,
            HelpStatus.NeedsHelp);

        var volunteer = await dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        volunteer!.AddPet(pet);
        await dbContext.SaveChangesAsync();

        return pet.Id.Value;
    }
    public static async Task<List<Guid>> SeedManyPets(
    WriteDbContext dbContext,
    Guid volunteerId,
    Guid speciesId,
    Guid breedId)
{
    var petSpeciesId = SpeciesId.Create(speciesId);
    var petBreedId = BreedId.Create(breedId);

    var pets = new List<Pet>
    {
        new Pet(
            PetId.NewPetId(),
            PetName.Create("Барсик").Value,
            PetDescription.Create("Умный кот").Value,
            PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
            PetColour.Create("Рыжий").Value,
            PetHealthInformation.Create("Здоровый кот").Value,
            Address.Create("ул. Тестовая, 1", "Москва", "Россия", "123456").Value,
            PetWeight.Create(4.5).Value,
            PetHeight.Create(25).Value,
            OwnersPhoneNumber.Create("79920013488").Value,
            true,
            Birthday.Create(new DateTime(2018, 5, 15)).Value,
            false,
            HelpStatus.NeedsHelp),
            
        new Pet(
            PetId.NewPetId(),
            PetName.Create("Мурзик").Value,
            PetDescription.Create("Ленивый кот").Value,
            PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
            PetColour.Create("Серый").Value,
            PetHealthInformation.Create("Нужен осмотр ветеринара").Value,
            Address.Create("ул. Тестовая, 2", "Москва", "Россия", "123456").Value,
            PetWeight.Create(5.2).Value,
            PetHeight.Create(28).Value,
            OwnersPhoneNumber.Create("79920013489").Value,
            true,
            Birthday.Create(new DateTime(2017, 3, 10)).Value,
            false,
            HelpStatus.FoundHome),
            
        new Pet(
            PetId.NewPetId(),
            PetName.Create("Шарик").Value,
            PetDescription.Create("Дружелюбный пёс").Value,
            PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
            PetColour.Create("Черный").Value,
            PetHealthInformation.Create("Аллергия на курицу").Value,
            Address.Create("ул. Тестовая, 3", "Москва", "Россия", "123456").Value,
            PetWeight.Create(12.7).Value,
            PetHeight.Create(45).Value,
            OwnersPhoneNumber.Create("79920013490").Value,
            true,
            Birthday.Create(new DateTime(2019, 7, 22)).Value,
            false,
            HelpStatus.NeedsHelp),
            
        new Pet(
            PetId.NewPetId(),
            PetName.Create("Рекс").Value,
            PetDescription.Create("Активная собака").Value,
            PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
            PetColour.Create("Белый").Value,
            PetHealthInformation.Create("Здоров").Value,
            Address.Create("ул. Тестовая, 4", "Москва", "Россия", "123456").Value,
            PetWeight.Create(15.3).Value,
            PetHeight.Create(50).Value,
            OwnersPhoneNumber.Create("79920013491").Value,
            true,
            Birthday.Create(new DateTime(2020, 2, 5)).Value,
            false,
            HelpStatus.NeedsHelp)
    };

    var volunteer = await dbContext.Volunteers
        .Include(v => v.Pets)
        .FirstOrDefaultAsync(v => v.Id == volunteerId);

    foreach (var pet in pets)
    {
        volunteer!.AddPet(pet);
    }

    await dbContext.SaveChangesAsync();

    return pets.Select(p => p.Id.Value).ToList();
}

    public static async Task<string> SeedPetPhoto(
        WriteDbContext dbContext,
        Guid volunteerId,
        Guid petId)
    {
        var volunteer = await dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        var pet = volunteer!.Pets
            .FirstOrDefault(p=>p.Id.Value == petId);

        var filePath = FilePath.Create("unnamed.jpg").Value;
        var photo=new Photo(filePath);
        
        pet!.AddPhoto(photo);
        
        await dbContext.SaveChangesAsync();

        return filePath.Path;
    }
}