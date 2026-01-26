using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Gorgonix_Back.Infrastructure.Services;

public class CloudinaryService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var acc = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<(string Url, string PublicId)> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face") // Opcional: redimensionar
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return (uploadResult.SecureUrl.ToString(), uploadResult.PublicId);
    }

    public async Task<(string Url, string PublicId)> AddVideoAsync(IFormFile file)
    {
        //TODO terminar
        
    }

    public async Task<string> DeleteVideoAsync(string file)
    {
        //TODO terminar
    }
    
    public async Task<string> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok" ? result.Result : null;
    }
}