using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyBookRequest
    {
        [Required(ErrorMessage = "Tên sách không được để trống")]
        [MaxLength(255, ErrorMessage = "Tên sách không được quá 255 ký tự")]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tác giả của sách không được để trống")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Thể loại của sách không được để trống")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Nhà xuất bản của sách không được để trống")]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "Năm xuất bản của sách không được để trống")]
        public int PublisherYear { get; set; }
    }
}
