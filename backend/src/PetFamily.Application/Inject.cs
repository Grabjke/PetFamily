using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Files.Presigned;
using PetFamily.Application.Files.Remove;
using PetFamily.Application.Files.Upload;
using PetFamily.Application.Volunteers.AddPets;
using PetFamily.Application.Volunteers.AddPhotoPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.MovePetPosition;
using PetFamily.Application.Volunteers.RemovePhotoPet;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<SoftDeleteVolunteerHandler>();
        services.AddScoped<RemoveFileHandler>();
        services.AddScoped<PresignedFileHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<MovePetPositionHandler>();
        services.AddScoped<AddPhotoPetHandler>();
        services.AddScoped<RemovePetPhotoHandler>();
        services.AddScoped<UploadFilesHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}