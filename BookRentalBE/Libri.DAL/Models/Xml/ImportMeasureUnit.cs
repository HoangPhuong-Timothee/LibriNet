namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class ImportMeasureUnit
    {
        public string SrcUnitName { get; set; }
        public string Description { get; set; }
        public decimal ConversionRate { get; set; }
        public string DestUnitName { get; set; }
    }
}
