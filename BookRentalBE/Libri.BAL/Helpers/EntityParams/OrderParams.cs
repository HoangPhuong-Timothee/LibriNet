namespace Libri.BAL.Helpers.EntityParams
{
    public class OrderParams : Params
    {
        public string? OrderEmail { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}