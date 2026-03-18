using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        [MaxLength(30, ErrorMessage = "Tên người dùng không được vượt quá 30 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).*$",
            ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Cần nhập lại mật khẩu để xác nhận")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^(\+84|0)\d{9,10}$", ErrorMessage = "Số điện thoại phải là một số ở Việt Name(+84 or 0)")]
        [MaxLength(11, ErrorMessage = "Số điện thoại chỉ tối đa 11 số")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Giới tính không được bỏ trống")]
        public string Gender { get; set; }
    }
}
