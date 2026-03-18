namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class MeasureUnit
    {
        public string SrcUnitName { get; set; }
        public string Description { get; set; }
        public decimal ConversionRate { get; set; }
        public int DestUnitId { get; set; }
    }
}
