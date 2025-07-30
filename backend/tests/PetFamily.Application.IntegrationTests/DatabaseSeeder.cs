using Microsoft.EntityFrameworkCore;
using PetFamily.Core;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Domain;
using PetFamily.Species.Infrastructure.DbContexts;
using PetFamily.Volunteers.Domain.PetManagement;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;
using PetFamily.Volunteers.Infrastructure.DbContexts;

namespace PetFamily.App.IntegrationTests;

public static class DatabaseSeeder
{
    public static async Task<Guid> SeedVolunteer(WriteVolunteerDbContext volunteerDbContext)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            FullName.Create("John", "Doe").Value,
            Email.Create("john@doe.com").Value,
            VolunteerDescription.Create("John Doe DESCRIPTION").Value,
            VolunteerExperience.Create(1).Value,
            OwnersPhoneNumber.Create("79920013488").Value);

        await volunteerDbContext.Volunteers.AddAsync(volunteer);
        await volunteerDbContext.SaveChangesAsync();

        return volunteer.Id;
    }

    public static async Task<(Guid SpeciesId, Guid BreedId)> SeedSpeciesAndBreed(WriteSpeciesDbContext dbContext)
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
        WriteVolunteerDbContext volunteerDbContext,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        var petSpeciesId = SpeciesId.Create(speciesId);
        var petBreedId = BreedId.Create(breedId);

        var pet = new Pet(
            PetId.NewPetId(),
            PetName.Create("Барсик").Value,
            Description.Create("Умный кот").Value,
            PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
            Colour.Create("Рыжий").Value,
            HealthInformation.Create("Здоровый кот").Value,
            Address.Create("ул. Тестовая", "Москва", "Россия", "123456").Value,
            Weight.Create(20).Value,
            Height.Create(30).Value,
            OwnersPhoneNumber.Create("79920013488").Value,
            true,
            Birthday.Create(new DateTime(2010, 1, 1)).Value,
            false,
            HelpStatus.NeedsHelp);

        var volunteer = await volunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        volunteer!.AddPet(pet);
        await volunteerDbContext.SaveChangesAsync();

        return pet.Id.Value;
    }

    public static async Task<List<Guid>> SeedManyPets(
        WriteVolunteerDbContext volunteerDbContext,
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
                Description.Create("Умный кот").Value,
                PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
                Colour.Create("Рыжий").Value,
                HealthInformation.Create("Здоровый кот").Value,
                Address.Create("ул. Тестовая, 1", "Москва", "Россия", "123456").Value,
                Weight.Create(4.5).Value,
                Height.Create(25).Value,
                OwnersPhoneNumber.Create("79920013488").Value,
                true,
                Birthday.Create(new DateTime(2018, 5, 15)).Value,
                false,
                HelpStatus.NeedsHelp),

            new Pet(
                PetId.NewPetId(),
                PetName.Create("Мурзик").Value,
                Description.Create("Ленивый кот").Value,
                PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
                Colour.Create("Серый").Value,
                HealthInformation.Create("Нужен осмотр ветеринара").Value,
                Address.Create("ул. Тестовая, 2", "Москва", "Россия", "123456").Value,
                Weight.Create(5.2).Value,
                Height.Create(28).Value,
                OwnersPhoneNumber.Create("79920013489").Value,
                true,
                Birthday.Create(new DateTime(2017, 3, 10)).Value,
                false,
                HelpStatus.FoundHome),

            new Pet(
                PetId.NewPetId(),
                PetName.Create("Шарик").Value,
                Description.Create("Дружелюбный пёс").Value,
                PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
                Colour.Create("Черный").Value,
                HealthInformation.Create("Аллергия на курицу").Value,
                Address.Create("ул. Тестовая, 3", "Москва", "Россия", "123456").Value,
                Weight.Create(12.7).Value,
                Height.Create(45).Value,
                OwnersPhoneNumber.Create("79920013490").Value,
                true,
                Birthday.Create(new DateTime(2019, 7, 22)).Value,
                false,
                HelpStatus.NeedsHelp),

            new Pet(
                PetId.NewPetId(),
                PetName.Create("Рекс").Value,
                Description.Create("Активная собака").Value,
                PetSpeciesBreed.Create(petSpeciesId, petBreedId).Value,
                Colour.Create("Белый").Value,
                HealthInformation.Create("Здоров").Value,
                Address.Create("ул. Тестовая, 4", "Москва", "Россия", "123456").Value,
                Weight.Create(15.3).Value,
                Height.Create(50).Value,
                OwnersPhoneNumber.Create("79920013491").Value,
                true,
                Birthday.Create(new DateTime(2020, 2, 5)).Value,
                false,
                HelpStatus.NeedsHelp)
        };

        var volunteer = await volunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        foreach (var pet in pets)
        {
            volunteer!.AddPet(pet);
        }

        await volunteerDbContext.SaveChangesAsync();

        return pets.Select(p => p.Id.Value).ToList();
    }

    public static async Task<string> SeedPetPhoto(
        WriteVolunteerDbContext volunteerDbContext,
        Guid volunteerId,
        Guid petId)
    {
        var volunteer = await volunteerDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        var pet = volunteer!.Pets
            .FirstOrDefault(p => p.Id.Value == petId);

        var filePath = FilePath.Create("unnamed.jpg").Value;
        var photo = new Photo(filePath);

        volunteer.AddPhotoPet(PetId.Create(petId), photo);

        await volunteerDbContext.SaveChangesAsync();

        return filePath.Path;
    }
}