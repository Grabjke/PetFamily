using AutoFixture;
using AutoFixture.AutoNSubstitute;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Commands.AddPet;
using PetFamily.Application.Volunteers.Commands.AddPhotoPet;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfoPet;
using PetFamily.Application.Volunteers.Commands.UpdateRequisites;
using PetFamily.Application.Volunteers.Commands.UpdateSocialNetworks;
using PetFamily.Domain.ValueObjects.Pet;

namespace PetFamily.App.IntegrationTests;

public static class FixtureExtensions
{
    public static AddPetCommand BuildAddPetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        return fixture.Build<AddPetCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(x => x.SpeciesId, speciesId)
            .With(x => x.BreedId, breedId)
            .With(x => x.Name, "Барсик")
            .With(x => x.Description, "Рыжий кот")
            .With(x => x.Colour, "Рыжий")
            .With(x => x.HealthInformation, "Заболевания отсутствуют")
            .With(x => x.Address,
                new AddressDto("ул. Тестовая", "Москва", "Россия", "123456"))
            .With(x => x.Weight, 55.5)
            .With(x => x.Height, 50)
            .With(x => x.OwnersPhoneNumber, "79920013477")
            .With(x => x.Castration, true)
            .With(x => x.Birthday, DateTime.Now.AddYears(-2))
            .With(x => x.isVaccinated, false)
            .With(x => x.HelpStatus, HelpStatus.NeedsHelp)
            .Create();
    }
    public static UpdateMainInfoPetCommand BuildUpdateMainInfoPetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid petId,
        Guid speciesId,
        Guid breedId)
    {
        return fixture.Build<UpdateMainInfoPetCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(x => x.SpeciesId, speciesId)
            .With(x => x.BreedId, breedId)
            .With(x => x.PetId, petId)
            .With(x => x.Name, "Update")
            .With(x => x.Description, "Update")
            .With(x => x.Colour, "Update")
            .With(x => x.HealthInformation, "Update")
            .With(x => x.Address,
                new AddressDto(
                    "Update ул. Тестовая",
                    " Update Москва", 
                    " UpdateРоссия", 
                    " Update123456"))
            .With(x => x.Weight, 55.5)
            .With(x => x.Height, 50)
            .With(x => x.OwnersPhoneNumber, "79920013577")
            .With(x => x.Castration, true)
            .With(x => x.Birthday, DateTime.Now.AddYears(-2))
            .With(x => x.isVaccinated, false)
            .With(x => x.HelpStatus, HelpStatus.NeedsHelp)
            .Create();
    }
    public static CreateVolunteerCommand BuildCreateVolunteerCommand(
        this IFixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>()
            .With(v => v.FullName, new FullNameDto("Joe", "Doe", null))
            .With(v => v.Email, "volunteer@example.com")
            .With(v => v.Description, "Опытный волонтер с 3 годами стажа")
            .With(v => v.Experience, 3)
            .With(v => v.PhoneNumber, "79920013477")
            .With(v => v.SocialNetworks, new List<SocialNetworkDto>
            {
                new SocialNetworkDto("VK", "https://vk.com/volunteer"),
                new SocialNetworkDto("Telegram", "https://t.me/volunteer")
            })
            .With(v => v.Requisites, new List<RequisitesDto>
            {
                new RequisitesDto("Сбербанк", "40817810550001234567")
            })
            .Create();
    }
    public static UpdateRequisitesCommand BuildUpdateRequisitesCommand(
        this IFixture fixture,
        Guid volunteerId)
    {
        return fixture.Build<UpdateRequisitesCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(v => v.Requisites, new List<RequisitesDto>
            {
                new RequisitesDto("Update Сбербанк", " Update40817810550001234567")
            })
            .Create();
    }
    public static UpdateSocialNetworksCommand BuildUpdateSocialNetworksCommand(
        this IFixture fixture,
        Guid volunteerId)
    {
        return fixture.Build<UpdateSocialNetworksCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(v => v.SocialNetworks, new List<SocialNetworkDto>
            {
                new("UpdateVK", "Updatehttps://vk.com/volunteer"),
                new("UpdateTelegram", "Updatehttps://t.me/volunteer")
            })
            .Create();
    }
    public static UpdateMainInfoCommand BuildUpdateMainInfoCommand(
        this IFixture fixture,
        Guid volunteerId)
    {
        return fixture.Build<UpdateMainInfoCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(v => v.FullName,
                new FullNameDto("Test", "Surname",""))
            .With(v => v.Email, "volunteer@exampleUpdate.com")
            .With(v => v.Description, "Опытный волонтер с 3 годами стажа")
            .With(v => v.Experience, 3)
            .With(v => v.PhoneNumber, "79920013477")
            .Create();
    }
    public static AddPhotoPetCommand BuildAddPhotoPetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid petId,
        int filesCount = 2)
    {
        fixture.Customize(new AutoNSubstituteCustomization());
        
        fixture.Register<Stream>(() => new MemoryStream(new byte[1024]));
        
        var files = fixture.Build<CreateFileDto>()
            .With(f => f.FileName, $"photo_{fixture.Create<string>()}.jpg")
            .CreateMany(filesCount)
            .ToList();
        
        return fixture.Build<AddPhotoPetCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(x => x.PetId, petId)
            .With(x => x.Files, files)
            .Create();
    }
    public static AddPhotoPetCommand BuildAddPhotoPetCommandWithInvalidFormat(
        this IFixture fixture,
        Guid volunteerId,
        Guid petId,
        int filesCount = 2)
    {
        fixture.Customize(new AutoNSubstituteCustomization());
        
        fixture.Register<Stream>(() => new MemoryStream(new byte[1024]));
        
        var files = fixture.Build<CreateFileDto>()
            .With(f => f.FileName, $"photo_{fixture.Create<string>()}.webp")
            .CreateMany(filesCount)
            .ToList();
        
        return fixture.Build<AddPhotoPetCommand>()
            .With(x => x.VolunteerId, volunteerId)
            .With(x => x.PetId, petId)
            .With(x => x.Files, files)
            .Create();
    }
    


}