using System.Data;
using System.Globalization;
using Dapper;
using Libri.BAL.Extensions;
using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomBook;
using Libri.DAL.Models.Custom.CustomError;
using Libri.DAL.Models.Xml;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Libri.BAL.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<BookService> _logger;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        public BookService(IPhotoService photoService, IUserService userService, IUnitOfWork uow, ILogger<BookService> logger)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
            _photoService = photoService;
        }

        public async Task<IEnumerable<BookWithDetailsAndTotalCount>> GetAllBooksAsync(BookParams bookParams)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Search", bookParams.Search, dbType: DbType.String, size: 255);
            parameters.Add("@Sort", bookParams.Sort, dbType: DbType.String, size: 50);
            parameters.Add("@GenreId", bookParams.GenreId, dbType: DbType.Int32);
            parameters.Add("@PublisherId", bookParams.PublisherId, dbType: DbType.Int32);
            parameters.Add("@AuthorId", bookParams.AuthorId, dbType: DbType.Int32);
            parameters.Add("@PageIndex", bookParams.PageIndex, dbType: DbType.Int16);
            parameters.Add("@PageSize", bookParams.PageSize, dbType: DbType.Int16);

            string spName = "sp_GetAllBooks";

            try
            {
               return await _uow.Dappers
                      .ExecuteStoreProcedureReturnAsync<BookWithDetailsAndTotalCount>(spName, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu sách: {@Exception}", e);
               
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu sách.", e);
            }
        }

        public async Task<IEnumerable<BookWithDetails>> GetLatestBooksAsync()
        {
            string spName = "sp_GetLatestBooks";

            try
            {
                return await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<BookWithDetails>(spName);
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu sách: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu sách.", e);
            }
        }

        public async Task<IEnumerable<BookWithDetails>> GetSimilarBooksAsync(int bookId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookId", bookId, dbType: DbType.Int32);

            string spName = "sp_GetSimilarBooks";

            try
            {
                return await _uow.Dappers
                     .ExecuteStoreProcedureReturnAsync<BookWithDetails>(spName, parameters);               
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu sách: {@Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu sách.", e);
            }
        }

        public async Task<BookWithDetails> GetBookByIdAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookId", id, dbType: DbType.Int32);

            string spName = "sp_GetBookByIdWithDetails";

            try
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<BookWithDetails>(spName, parameters);

                var book = result.FirstOrDefault();

                if (book == null)
                {
                    _logger.LogError("Không tìm thấy sách có mã {bookId}.", id);
                }

                return book;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu sách: {useException}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu sách.", e);
            }
        }

        public async Task<StoreProcedureResult> ImportBooksFromFileAsync(IFormFile file)
        {
            var response = new StoreProcedureResult();
            string userName = _userService.GetUserNameFromClaims();

            var (errors, entities) = await ValidateAndConvertToXmlBookFileAsync(file);

            if (errors.Any())
            {
                response.Message = "Có lỗi xảy ra khi chuyển đổi dữ liệu từ file";
                response.Success = false;
                response.Errors = errors;
                return response;
            }

            var booksXml = entities.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@BooksXml", booksXml, dbType: DbType.Xml);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_BulkUpsertBooks";

            try
            {
                var result = await _uow.Dappers
                           .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                response.Message = result.FirstOrDefault().Success ? result.FirstOrDefault().Message : "Có lỗi xảy ra trong quá trình nhập dữ liệu."; 
                response.Success = result.FirstOrDefault().Success; 
                response.Errors = result.FirstOrDefault().Errors;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm sách: {Exception}", e);
               
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi thêm sách.", e);
            }
        }

        public async Task<StoreProcedureResult> DeleteBookAsync(int id)
        {
            string userName = _userService.GetUserNameFromClaims();

            var parameters = new DynamicParameters();
            parameters.Add("@BookId", id, dbType: DbType.Int32);
            parameters.Add("@UserName", userName, dbType: DbType.String, size: 255);

            string spName = "sp_SoftDeleteBookById";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi xóa sách: {Exception}", e);
               
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi xóa sách.", e);
            }
        }

        public async Task<bool> CheckBookExistByTitleAsync(string bookTitle)
        {
            try
            {
               var result = await _uow.Books
                    .Queryable()
                    .Where(b => b.Title!.ToLower().Trim() == bookTitle.ToLower().Trim())
                    .AnyAsync();

                return !result;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi lấy dữ liệu sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi lấy dữ liệu sách.", e);
            }
        }

        public async Task<bool> CheckBookExistInBookStoreAsync(string bookTitle, int bookStoreId) 
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookStoreId", bookStoreId, dbType: DbType.Int32);
            parameters.Add("@BookTitle", bookTitle, dbType: DbType.String, size: 255);

            string spName = "sp_CheckBookExistInBookStore";

            try 
            {
                var result = await _uow.Dappers
                    .ExecuteStoreProcedureReturnAsync<int>(spName, parameters);

                return result.FirstOrDefault() == 1;
            }
            catch (Exception e) 
            {
                _logger.LogError("Lỗi kiểm tra dữ liệu sách trong hiệu sách: {Exception}", e);
                
                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi kiểm tra dữ liệu sách trong hiệu sách.", e);
            }
        }

        public async Task<bool> CheckBookISBNAsync(string isbn, string bookTitle)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ISBN", isbn, dbType: DbType.String, size: 100);
            parameters.Add("@BookTitle", bookTitle, dbType: DbType.String, size: 255);

            string spName = "sp_CheckBookISBN";
            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<int>(spName, parameters);

                return result.FirstOrDefault() == 1;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi kiểm tra ISBN của sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi kiểm tra ISBN của sách.", e);
            }
        }

        public async Task<StoreProcedureResult> UploadBookImagesAsync(int bookId, List<IFormFile> files)
        {
            var response = new StoreProcedureResult();
            var uploadErrors = new List<ErrorDetails>();
            var bookImages = new List<Image>();
            int userId = _userService.GetUserIdFromClaims();

            int maxFileUpload = 5;

            if (files == null || files.Count <= 0)
            {
                response.Message = "Chưa có file nào được tải lên";
                response.Success = false;
                return response;
            }

            if (files.Count > maxFileUpload)
            {
                response.Message = "Chỉ có thể tải lên tối đa 5 ảnh cho mỗi sản phẩm";
                response.Success = false;
                return response;
            }

            foreach (var file in files)
            {
                if (!ValidateImageFile(file, out string errorMessage))
                {
                    uploadErrors.Add(new ErrorDetails
                    {
                        Location = $"Ảnh '{file.Name}'",
                        Details = errorMessage
                    });
                    response.Message = "Một số file tải lên không hợp lệ";
                    response.Success = false;
                    response.Errors = uploadErrors;
                    return response;
                }

                var (uploadedImage, uploadError) = await UploadImageAsync(file);

                if (uploadError != null)
                {
                    uploadErrors.Add(new ErrorDetails
                    {
                        Location = $"Ảnh '{file.Name}'",
                        Details = uploadError
                    });
                    response.Message = "Có lỗi xảy ra khi tải ảnh lên";
                    response.Success = false;
                    response.Errors = uploadErrors;
                    return response;
                }

                bookImages.Add(uploadedImage!);
            }

            var bookImagesXml = bookImages.ToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@BookId", bookId, dbType: DbType.Int32);
            parameters.Add("@UserId", userId, dbType: DbType.Int32);
            parameters.Add("@BookImages", dbType: DbType.Xml);

            string spName = "sp_UploadBookImages";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi thêm ảnh mô tả sách: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra khi thêm ảnh mô tả sách.", e);
            }
        }

        private static bool ValidateImageFile(IFormFile file, out string errorMessage)
        {
            string[] allowedFileTypes = { "image/jpeg", "image/png", "image/jpg" };
            long maxFileSize = 2 * 1024 * 1024;
            
            if (!allowedFileTypes.Contains(file.ContentType)) 
            { 
                errorMessage = "Định dạng file không hợp lệ. Vui lòng chọn file ảnh có định dạng jpeg, jpg hoặc png"; 
                return false; 
            }
            
            if (file.Length > maxFileSize) 
            { 
                errorMessage = "Dung lượng file quá lớn. Vui lòng chọn file ảnh có dung lượng nhỏ hơn 2MB"; 
                return false; 
            }

            errorMessage = string.Empty; 
            return true;
        }

        private async Task<(Image? image, string? errorMessage)> UploadImageAsync(IFormFile file)
        {
            var uploadImageResult = await _photoService.UploadImageAsync(file);

            if (uploadImageResult.Error != null)
            {
                return (null, uploadImageResult.Error.Message);
            }

            var image = new Image
            {
                PublicId = uploadImageResult.PublicId,
                ImageUrl = uploadImageResult.SecureUrl.AbsoluteUri
            };

            return (image, null);
        }

        private static async Task<(List<ErrorDetails> errors, List<Book> entities)> ValidateAndConvertToXmlBookFileAsync(IFormFile file)
        {
            var requiredColumns = new List<string>
            {
                "Tiêu đề",
                "Tác giả",
                "Thể loại",
                "NXB",
                "Mô tả",
                "Ảnh minh họa",
                "Giá",
                "Năm xuất bản"
            };

            var columnDataType = new Dictionary<string, string>
            {
                { "Tiêu đề", "chuỗi ký tự" },
                { "Tác giả", "chuỗi ký tự" },
                { "Thể loại", "chuỗi ký tự" },
                { "NXB", "chuỗi ký tự" },
                { "Mô tả", "chuỗi ký tự" },
                { "Ảnh minh họa", "chuỗi ký tự" },
                { "Giá", "số thập phân" },
                { "Năm xuất bản", "số nguyên" }
            };

            var rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>
            {
                {
                    "Tiêu đề",
                    (value => !string.IsNullOrEmpty(value),
                        "Tiêu đề không được để trống"
                    )
                },
                {
                    "Tác giả",
                    (value => !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Tác giả không được để trống và chỉ chấp nhận ký tự")
                },
                {
                    "Thể loại",
                    (value => !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "Thể loại không được để trống và không chấp nhận số")
                },
                {
                    "NXB",
                    (value => !string.IsNullOrEmpty(value) && !value.Any(char.IsDigit),
                        "NXB không được để trống và không chứa số")
                },
                {
                    "Mô tả",
                    (value => !string.IsNullOrEmpty(value),
                        "Mô tả không được để trống")
                },
                {
                    "Ảnh minh họa",
                    (value => !string.IsNullOrEmpty(value),
                        "Ảnh minh họa không được để trống")
                },
                {
                    "Giá",
                    (value =>
                        {
                            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                            {
                                return price >= 0;
                            }
                            return false;
                        },
                        "Giá phải là số và lớn hơn hoặc bằng 0"
                    )
                },
                {
                    "Năm xuất bản",
                    (value =>
                        int.TryParse(value, out var year) && year > 0 && year <= DateTime.Now.Year,
                        "Năm xuất bản phải là số và không lớn hơn năm hiện tại")
                }
            };

            var headerMapping = new Dictionary<string, string>
            {
                { "Tiêu đề", "Title" },
                { "Tác giả", "Author" },
                { "Thể loại", "Genre" },
                { "NXB", "Publisher" },
                { "Mô tả", "Description" },
                { "Ảnh minh họa", "ImageUrl" },
                { "Giá", "Price" },
                { "Năm xuất bản", "PublishYear" }
            };

            var fileValidation = new FileValidation
            {
                RequiredColumns = requiredColumns,
                ColumnDataTypes = columnDataType,
                Rules = rules
            };

            return await FileExtensions.ConvertFileDataToObjectModelAsync<Book>(file, fileValidation, headerMapping);
        }
    }
}