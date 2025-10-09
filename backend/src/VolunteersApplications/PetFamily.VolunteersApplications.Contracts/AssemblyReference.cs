using System.Reflection;

namespace PetFamily.VolunteersApplications.Contracts;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}