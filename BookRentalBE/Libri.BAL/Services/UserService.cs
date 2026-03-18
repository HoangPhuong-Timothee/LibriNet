using Dapper;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomUser;
using Libri.DAL.Models.Domain;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Security.Claims;
using Libri.BAL.Extensions;
using Libri.DAL.Models.Xml;
using Libri.DAL.Models.Custom;
using System.Drawing.Imaging;
using System.Drawing;

namespace Libri.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IPhotoService _photoService;
        private readonly ILogger<UserService> _logger;
        public UserService(IUnitOfWork uow, ILogger<UserService> logger, IHttpContextAccessor httpContext, IPhotoService photoService)
        {
            _uow = uow;
            _httpContext = httpContext;
            _logger = logger;
            _photoService = photoService;
        }

        public async Task<IEnumerable<MemberWithTotalCount>> GetUsersListAsync(UserParams userParams)
        {
            int userId = GetUserIdFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, dbType: DbType.Int16);
            parameters.Add("@PageIndex", userParams.PageIndex, dbType: DbType.Int16);
            parameters.Add("@PageSize", userParams.PageSize, dbType: DbType.Int16);

            string spName = "sp_GetUsersList";

            try
            {
                 return await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<MemberWithTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu danh sách người dùng: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu danh sách người dùng", e);
            }
        }

        public async Task<IEnumerable<CurrentUser>> GetUserByEmailAsync(string email)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email, dbType: DbType.String, size: 255);

            string spName = "sp_GetUserByEmail";

            try
            {
                var result = await _uow.Dappers
                       .ExecuteStoreProcedureReturnAsync<CurrentUser>(spName, parameters);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi tìm kiếm người dùng theo email: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình tìm kiếm người dùng theo email", e);
            }
        }

        public async Task<IEnumerable<CurrentUser>> GetUserByIdAsync()
        {
            var userId = GetUserIdFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, dbType: DbType.Int16);

            string spName = "sp_GetCurrentUserById";

            try
            {
                var result = await _uow.Dappers
                       .ExecuteStoreProcedureReturnAsync<CurrentUser>(spName, parameters);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi tìm kiếm người dùng theo mã: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình tìm kiếm người dùng theo mã", e);
            }
        }

        public async Task<Address?> GetAddressByUserIdAsync()
        {
            int userId = GetUserIdFromClaims();

            try
            {
                var userAddress = await _uow.UserAddresses
                    .Queryable()
                    .Where(a => a.UserId == userId)
                    .FirstOrDefaultAsync();
                
                if (userAddress == null)
                {
                    _logger.LogInformation("Người dùng chưa cập nhật địa chỉ");
                    return null;
                }

                return userAddress;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thông tin địa chỉ người dùng: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình lấy thông tin địa chỉ người dùng", e);
            }
        }

        public async Task<StoreProcedureResult> UpdateUserProfileAsync(Profile profile)
        {
            int userId = GetUserIdFromClaims();

            string profileXml = profile.SingleObjectToXml();
            
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, dbType: DbType.Int32);
            parameters.Add("ProfileXml", profileXml, dbType: DbType.Xml);
            
            string spName = "sp_UpdateUserInfo";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật thông tin cá nhân: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException?.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật thông tin cá nhân", e);
            }
        }

        public async Task<StoreProcedureResult> UpdateUserAddressAsync(Address address)
        {
            int userId = GetUserIdFromClaims();
            
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, dbType: DbType.Int32);
            parameters.Add("@FullName", address.FullName, dbType: DbType.String, size: 255);
            parameters.Add("@Street", address.Street, dbType: DbType.String, size: 255);
            parameters.Add("@Ward", address.Ward, dbType: DbType.String, size: 255);
            parameters.Add("@District", address.District, dbType: DbType.String, size: 255);
            parameters.Add("@City", address.City, dbType: DbType.String, size: 255);
            parameters.Add("@PostalCode", address.PostalCode, dbType: DbType.String, size: 20);

            string spName = "sp_ModifyUserAddressByUserId";
            
            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);
                
                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật thông tin địa chỉ: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException?.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật thông tin địa chỉ", e);
            }
        }

        public async Task<StoreProcedureResult> UploadUserImageAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();

            string[] allowedFileTypes = { "image/jpeg", "image/png", "image/jpg" };
            long maxFileSize = 2 * 1024 * 1024;

            if (file == null)
            {
                response.Message = "Chưa có file nào được tải lên";
                response.Success = false;
                return response;
            }

            if (!allowedFileTypes.Contains(file.ContentType))
            {
                response.Message = "Định dạng file không hợp lệ. Vui lòng chọn file ảnh có định dạng jpeg, jpg hoặc png.";
                response.Success = false;
                return response;
            }

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var imageBytes = ms.ToArray();

                if (imageBytes.Length > maxFileSize)
                {
                    using (var originalImage = System.Drawing.Image.FromStream(new MemoryStream(imageBytes)))
                    {
                        var ratio = Math.Sqrt((double)maxFileSize / imageBytes.Length);
                        var newWidth = (int)(originalImage.Width * ratio);
                        var newHeight = (int)(originalImage.Height * ratio);

                        using (var resizedImage = new Bitmap(originalImage, newWidth, newHeight))
                        {
                            using (var outputMs = new MemoryStream())
                            {
                                var encoderParams = new EncoderParameters(1);
                                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 75L);

                                var jpegEncoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                                resizedImage.Save(outputMs, jpegEncoder, encoderParams);

                                imageBytes = outputMs.ToArray();
                            }
                        }
                    }
                }

                file = new FormFile(new MemoryStream(imageBytes), 0, imageBytes.Length, file.Name, file.FileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = file.ContentType
                };
            }

            int userId = GetUserIdFromClaims();
            var userInfo = await _uow.UserInfos.GetByIdAsync(userId);

            if (userInfo!.ImagePublicId != null)
            {
                var deleteOldImage = await _photoService.DeleteImageAsync(userInfo!.ImagePublicId!);

                if (deleteOldImage.Error != null)
                {
                    response.Message = deleteOldImage.Error.Message;
                    response.Success = false;
                    return response;
                }
            }

            var uploadImageResult = await _photoService.UploadImageAsync(file);

            if (uploadImageResult.Error != null)
            {
                response.Message = uploadImageResult.Error.Message;
                response.Success = false;
                return response;
            }

            string imagePublicId = uploadImageResult.PublicId;
            string imageUrl = uploadImageResult.SecureUrl.AbsoluteUri;

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, dbType: DbType.Int32);
            parameters.Add("@ImagePublicId", imagePublicId, dbType: DbType.String, size: 255);
            parameters.Add("@ImageUrl", imageUrl, dbType: DbType.String, size: 255);

            string spName = "sp_ModifyUserImageByUserId";

            try
            {
                var result = await _uow.Dappers.ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi cập nhật ảnh đại diện người dùng: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException?.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật ảnh đại diện của người dùng", e);
            }
        }

        public int GetUserIdFromClaims()
        {
            var httpContext = _httpContext?.HttpContext;
            
            if (httpContext == null || httpContext.User == null)
            {
                _logger.LogError("Không tìm thấy người dùng hoặc http context");
                throw new Exception("Người dùng hoặc http context không hợp lệ.");
            }

            var userId = httpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("Không tìm thấy người dùng ứng với mã {userId}.", userId);
                throw new Exception("Không tìm thấy người dùng.");
            }

            return int.Parse(userId);
        }

        public string GetUserNameFromClaims()
        {
            var httpContext = _httpContext?.HttpContext;

            if (httpContext == null || httpContext.User == null)
            {
                _logger.LogError("Không tìm thấy người dùng hoặc http context");
                throw new Exception("Người dùng hoặc http context không hợp lệ.");
            }

            var userName = httpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogError("Không tìm thấy người dùng có tên {username}.", userName);
                throw new Exception("Không tìm thấy người dùng.");
            }

            return userName;
        }

        public string GetUserEmailFromClaims()
        {
            var httpContext = _httpContext?.HttpContext;
            
            if (httpContext == null || httpContext.User == null)
            {
                _logger.LogError("Không tìm thấy người dùng hoặc http context");
                throw new Exception("Người dùng hoặc http context không hợp lệ.");
            }

            var email = httpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Không tìm thấy người dùng ứng với email {userEmail}.", email);
                throw new Exception("Không tìm thấy người dùng.");
            }
            
            return email;
        }
    }
}
