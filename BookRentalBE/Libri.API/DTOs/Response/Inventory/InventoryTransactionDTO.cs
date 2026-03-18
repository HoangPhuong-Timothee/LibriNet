namespace Libri.API.DTOs.Response.Inventory
{
    public class InventoryTransactionDTO
    {
        public int TransactionId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public string TransactionDate { get; set; }
        public string PerformedBy { get; set; }
        public string TransactionNotes { get; set; }
    }
}