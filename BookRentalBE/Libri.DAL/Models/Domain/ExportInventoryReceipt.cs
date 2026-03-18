namespace Libri.DAL.Models.Domain
{
    public partial class ExportInventoryReceipt
    {
        public ExportInventoryReceipt()
        {
            ExportReceiptItems = new HashSet<ExportReceiptItem>();
        }

        public int Id { get; set; }
        public string ExportReceiptCode { get; set; } = null!;
        public int TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? ExportNotes { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual ICollection<ExportReceiptItem> ExportReceiptItems { get; set; }
    }
}
