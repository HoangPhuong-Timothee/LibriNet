namespace Libri.BAL.Helpers.EntityParams
{
    public class InventoryReceiptParams : Params
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ReceiptStatus { get; set; }
        public string? ReceiptType { get; set; }
    }
}
