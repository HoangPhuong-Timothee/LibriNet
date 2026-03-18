using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyAuthorRequest
    {
        [Required(ErrorMessage = "Author name can not be empty.")]
        [MaxLength(50, ErrorMessage = "Author name can be over 50 characters.")]
        public string Name { get; set; }
    }
}
