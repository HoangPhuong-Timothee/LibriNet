using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
