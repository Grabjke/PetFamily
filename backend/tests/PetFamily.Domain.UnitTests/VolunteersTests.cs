

using FluentAssertions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.PetManagement;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Pet;
using PetFamily.Volunteers.Domain.PetManagement.ValueObjects.Volunteer;

namespace PetFamily.UnitTests;

public class VolunteersTests
{
    [Fact]
    public void Pet_should_remain_in_place_when_new_position_equals_current()
    {
        const int petsCount = 5;
        var sut = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(2).Value;
        var firstPet = sut.Pets[0];
        var secondPet = sut.Pets[1];
        var thirdPet = sut.Pets[2];
        var fourthPet = sut.Pets[3];
        var fifthPet = sut.Pets[4];

        var result = sut.MovePet(secondPet, positionTo);

        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Pet_should_move_forward_and_shift_others_when_new_position_is_lower()
    {
        const int petsCount = 5;
        var sut = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(2).Value;
        var firstPet = sut.Pets[0];
        var secondPet = sut.Pets[1];
        var thirdPet = sut.Pets[2];
        var fourthPet = sut.Pets[3];
        var fifthPet = sut.Pets[4];

        var result = sut.MovePet(fourthPet, positionTo);

        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Pet_should_move_back_and_shift_others_when_new_position_is_higher()
    {
        const int petsCount = 5;
        var sut = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(4).Value;
        var firstPet = sut.Pets[0];
        var secondPet = sut.Pets[1];
        var thirdPet = sut.Pets[2];
        var fourthPet = sut.Pets[3];
        var fifthPet = sut.Pets[4];

        var result = sut.MovePet(secondPet, positionTo);

        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }
    [Fact]
    public void Pet_should_become_first_and_shift_others_forward_when_moved_to_start()
    {
        const int petsCount = 5;
        var sut = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(1).Value;
        var firstPet = sut.Pets[0];
        var secondPet = sut.Pets[1];
        var thirdPet = sut.Pets[2];
        var fourthPet = sut.Pets[3];
        var fifthPet = sut.Pets[4];

        var result = sut.MovePet(fifthPet, positionTo);
        
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void Pet_should_become_last_and_shift_others_back_when_moved_to_end()
    {
        const int petsCount = 5;
        var sut = CreateVolunteerWithPets(petsCount);
        var positionTo = Position.Create(5).Value;
        var firstPet = sut.Pets[0];
        var secondPet = sut.Pets[1];
        var thirdPet = sut.Pets[2];
        var fourthPet = sut.Pets[3];
        var fifthPet = sut.Pets[4];

        var result = sut.MovePet(firstPet, positionTo);
        
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
        var phoneNumber = OwnersPhoneNumber.Create("79920013488").Value;
        
        var volunteer = new Volunteer(
            VolunteerId.NewVolunteerId(),
            fullName,
            email,
            volunteerDescription,
            volunteerExperience,
            phoneNumber);

        var petName = PetName.Create("Test").Value;
        var petDescription = Description.Create("Test").Value;
        var petSpeciesBreed = PetSpeciesBreed.Create(SpeciesId.NewSpeciesId(), BreedId.NewBreedId()).Value;
        var petColour = Colour.Create("Test").Value;
        var petHealthInformation = HealthInformation.Create("Test").Value;
        var address = Address.Create("Test", "Test", "Test", "Test").Value;
        var petWeight = Weight.Create(1).Value;
        var petHeight = Height.Create(1).Value;
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