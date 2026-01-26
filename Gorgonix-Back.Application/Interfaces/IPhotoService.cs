using Microsoft.AspNetCore.Http;

namespace Gorgonix_Back.Application.Interfaces;

public interface IPhotoService
{
    //portadas
    Task<(string Url, string PublicId)> AddPhotoAsync(IFormFile file);
    Task<string> DeletePhotoAsync(string publicId);

    //"peliculas"
    Task<(string Url, string PublicId)> AddVideoAsync(IFormFile file);
    Task<string> DeleteVideoAsync(string publicId);
}