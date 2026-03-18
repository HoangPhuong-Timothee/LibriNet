using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Libri.BAL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Libri.BAL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ILogger<PhotoService> _logger;
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;
        public PhotoService(ILogger<PhotoService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            Account account = new Account
            (
                _config["Cloudinary:CloudName"],
                _config["Cloudinary:APIKey"],
                _config["Cloudinary:APISecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            try
            {
                var uploadResult = new ImageUploadResult();
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    }
                }
                return uploadResult;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi tải lên file ảnh: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình tải lên file ảnh.", e);
            }
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                
                return deletionResult;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa file ảnh: {Exception}", e);
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }
                throw new Exception("Có lỗi xảy ra trong quá trình xóa file ảnh.", e);
            }
        }
    }
}
