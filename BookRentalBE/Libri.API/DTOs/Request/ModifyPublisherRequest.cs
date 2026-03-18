using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyPublisherRequest
    {
        [Required(ErrorMessage = "Tên nhà xuất bản không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên nhà xuất bản không được quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ của nhà xuất bản không được để trống.")]
        public string Address { get; set; }
    }
}
