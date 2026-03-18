using Libri.DAL.Models.Custom.CustomError;

namespace Libri.API.DTOs.Response.Errors
{
    public class APIResponse
    {
        public APIResponse(int statusCode, string message = null, IEnumerable<ErrorDetails> errors = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Errors = errors;
        }
        
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<ErrorDetails> Errors { get; set; }
        
        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bạn vừa làm một cái yêu cầu tồi tệ",
                401 => "Đã xác thực danh tính chưa ?",
                403 => "Có quyền không mà muốn làm cái này ?",
                404 => "Có đâu mà kiếm",
                500 => "Chào mừng tới địa ngục, hố đen. Có lỗi thì giận. Giận thì mất khôn. Mất khôn thì phá. Phá thì thất nghiệp",
                _ => null,
            };
        }
    }
}
