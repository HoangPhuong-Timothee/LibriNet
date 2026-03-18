using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class AddExportInventoryReceiptRequest
    {
        [Required]
        public string ExportNotes { get; set; }
        [Required]
        public List<ExportReceiptItemDTO> ExportReceiptItems { get; set; } = new List<ExportReceiptItemDTO>();
    }

    public class ExportReceiptItemDTO
    {
        [Required(ErrorMessage = "Tiêu đề sách không được để trống")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "Số lượng nhập kho không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng nhập kho phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn vị xuất kho không được để trống")]
        public int UnitOfMeasureId { get; set; }

        [Required(ErrorMessage = "Mã hiệu sách không được để trống")]
        public int BookStoreId { get; set; }

        [Required(ErrorMessage = "ISBN của sách không được để trống")]
        public string ISBN { get; set; }
    }
}
