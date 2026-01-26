using Microsoft.AspNetCore.Http;

namespace Gorgonix_Back.Application.Interfaces;

public interface IPhotoService
{
    Task<(string Url, string PublicId)> AddPhotoAsync(IFormFile file);
    Task<string> DeletePhotoAsync(string publicId);
}