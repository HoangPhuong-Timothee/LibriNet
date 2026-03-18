namespace Libri.API.DTOs.Response.Inventory
{
    public class InventoryReceiptDTO
    {
        public int ReceiptId { get; set; }
        public string ReceiptCode { get; set; }
        public int TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImportNotes { get; set; }
        public string ReceiptStatus { get; set; }
        public string ReceiptType { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateInfo { get; set; }
    }
}
