using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyGenreRequest
    {
        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        [MaxLength(20, ErrorMessage = "Tên thể loại không được quá 20 ký tự")]
        public string Name { get; set; }
    }
}
