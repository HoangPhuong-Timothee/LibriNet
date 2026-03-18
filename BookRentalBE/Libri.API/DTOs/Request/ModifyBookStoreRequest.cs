using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyBookStoreRequest
    {
        [Required(ErrorMessage = "Tên hiệu sách không được để trống")]
        [MaxLength(255, ErrorMessage = "Tên hiệu sách không được quá 255 ký tự")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string StoreAddress { get; set; }
    }
}
