using Microsoft.AspNetCore.Http;

namespace Gorgonix_Back.Application.Interfaces;

public interface IMediaService
{
    Task<(string Url, string PublicId)> AddPhotoAsync(IFormFile file);
    Task<string> DeletePhotoAsync(string publicId);
    Task<(string Url, string PublicId)> AddVideoAsync(IFormFile file);
    Task<string> DeleteVideoAsync(string publicId);
}