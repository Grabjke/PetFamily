using Microsoft.AspNetCore.Http;
using PetFamily.Core.Dtos;

namespace PetFamily.Core;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileDto> _files = [];

    public List<CreateFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new CreateFileDto(stream, file.FileName);
            _files.Add(fileDto);
        }

        return _files;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var file in _files)
        {
            await file.Content.DisposeAsync();
        }
    }
}