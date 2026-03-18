namespace Libri.DAL.Models.Domain
{
    public class UnitMapping
    {
        public int SrcUnitId { get; set; }
        public decimal SrcUnitRate { get; set; }
        public int DestUnitId { get; set; }
        public decimal DestUnitRate { get; set; }

        public virtual UnitOfMeasure DestUnit { get; set; } = null!;
        public virtual UnitOfMeasure SrcUnit { get; set; } = null!;
    }
}
