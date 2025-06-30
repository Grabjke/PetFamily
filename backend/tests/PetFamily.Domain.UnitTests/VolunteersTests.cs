using FluentAssertions;
using PetFamily.Domain.AggregateRoots;
using PetFamily.Domain.Entities;
using PetFamily.Domain.ValueObjects.Breed;
using PetFamily.Domain.ValueObjects.Pet;
using PetFamily.Domain.ValueObjects.Species;
using PetFamily.Domain.ValueObjects.Volunteer;

namespace PetFamily.UnitTests;

public class VolunteersTests
{
    [Fact]
    public void Move_Pet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(2).Value;
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        var result = volunteer.MovePet(secondPet, positionTo);

        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Forward_When_New_Position_Is_Lower()
    {
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(2).Value;
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        var result = volunteer.MovePet(fourthPet, positionTo);

        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Is_Greater()
    {
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(4).Value;
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        var result = volunteer.MovePet(secondPet, positionTo);

        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }
    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Forward_When_New_Position_Is_First()
    {
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(1).Value;
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        var result = volunteer.MovePet(fifthPet, positionTo);
        
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pets_Back_When_New_Position_Is_Last()
    {
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(5).Value;
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        var result = volunteer.MovePet(firstPet, positionTo);
        
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
    }
    private Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var fullName = FullName.Create("Test", "Test").Value;
        var email = Email.Create("Test@gmail.com").Value;
        var volunteerDescription = VolunteerDescription.Create("Test").Value;
        var volunteerExperience = VolunteerExperience.Create(3).Value;
        var phoneNumber = OwnersPhoneNumber.Create("9920013488").Value;
        
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            fullName,
            email,
            volunteerDescription,
            volunteerExperience,
            phoneNumber);

        var petName = PetName.Create("Test").Value;
        var petDescription = PetDescription.Create("Test").Value;
        var petSpeciesBreed = PetSpeciesBreed.Create(SpeciesId.NewPetId(), BreedId.NewPetId()).Value;
        var petColour = PetColour.Create("Test").Value;
        var petHealthInformation = PetHealthInformation.Create("Test").Value;
        var address = Address.Create("Test", "Test", "Test", "Test").Value;
        var petWeight = PetWeight.Create(1).Value;
        var petHeight = PetHeight.Create(1).Value;
        var birthDay = Birthday.Create(new DateTime(2006, 5, 15)).Value;
        
        for (int i = 0; i < petsCount; i++)
        {
            var pet = new Pet(
                PetId.NewPetId(),
                petName,
                petDescription,
                petSpeciesBreed,
                petColour,
                petHealthInformation,
                address,
                petWeight,
                petHeight,
                phoneNumber,
                true,
                birthDay,
                true,
                HelpStatus.FoundHome);

            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}