namespace Libri.BAL.Helpers.EntityParams
{
    public class InventoryTransactionParams
    {
        public int BookId { get; set; }
        public string StoreName { get; set; }
        public int? MeasureUnitId { get; set; }
        public string? TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }  
    }
}