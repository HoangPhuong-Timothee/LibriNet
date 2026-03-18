using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class AddImportInventoryReceiptRequest
    {
        [Required]
        public string ImportNotes { get; set; }
        [Required]
        public List<ImportReceiptItemDTO> ImportReceiptItems { get; set; } = new List<ImportReceiptItemDTO>(); 
    }

    public class ImportReceiptItemDTO 
    {
        [Required(ErrorMessage = "Tiêu đề sách không được để trống")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "Số lượng nhập kho không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng nhập kho phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn vị nhập kho không được để trống")]
        public int UnitOfMeasureId { get; set; }

        [Required(ErrorMessage = "Mã hiệu sách không được để trống")]
        public int BookStoreId { get; set; }

        [Required(ErrorMessage = "ISBN của sách không được để trống")]
        public string ISBN { get; set; }
    }
}
