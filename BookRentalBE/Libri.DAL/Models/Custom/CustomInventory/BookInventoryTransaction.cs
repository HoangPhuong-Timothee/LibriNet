namespace Libri.DAL.Models.Custom.CustomInventory
{
    public class BookInventoryTransaction
    {
        public int TransactionId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PerformedBy { get; set; }
        public string TransactionNotes { get; set; }
    }
}