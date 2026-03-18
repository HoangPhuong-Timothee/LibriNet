namespace Libri.BAL.Helpers.EntityParams
{
    public class InventoryAuditParams : Params
    {
        public string? AudittedBy { get; set; }
        public string? AuditStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
