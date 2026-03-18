using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class AddInventoryAuditRequest
    {       
        [Required(ErrorMessage = "Người thực hiện kiểm kê không được để trống")]
        public string AudittedBy { get; set; }
        
        [Required(ErrorMessage = "Ngày kiểm kê không được để trống")]
        public DateTime AuditDate { get; set; }

        [MaxLength(100, ErrorMessage = "Ghi chú kiểm kê không được quá 100 ký tự")]
        public string AuditNotes { get; set; }

        [Required]
        public List<InventoryAuditItemDTO> InventoryAuditItems { get; set; } = new List<InventoryAuditItemDTO>();
    }

    public class InventoryAuditItemDTO
    {
        [Required]
        public string BookTitle { get; set; }
        
        [Required]
        public string ISBN { get; set; }

        [Required]
        public int BookStoreId { get; set; }
    }
}
