namespace PetFamily.Framework.Authorization;

public static class Permissions
{
    public static class Volunteers
    {
        public const string DeletePet = "pet.delete";
        public const string CreatePet = "pet.create";
        public const string PetRead = "pet.read";
    }
}