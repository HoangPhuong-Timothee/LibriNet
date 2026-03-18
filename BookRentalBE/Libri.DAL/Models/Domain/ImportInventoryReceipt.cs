namespace Libri.DAL.Models.Domain
{
    public partial class ImportInventoryReceipt
    {
        public ImportInventoryReceipt()
        {
            ImportReceiptItems = new HashSet<ImportReceiptItem>();
        }

        public int Id { get; set; }
        public string ImportReceiptCode { get; set; } = null!;
        public int TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImportNotes { get; set; } = null!;

        public virtual ICollection<ImportReceiptItem> ImportReceiptItems { get; set; }
    }
}
