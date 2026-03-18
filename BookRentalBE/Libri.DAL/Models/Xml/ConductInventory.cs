namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class ConductInventory
    {
        public string BookTitle { get; set; }

        public string ISBN { get; set; }

        public string StoreName { get; set; }

        public int ActualQuantity { get; set; }
        public int UnitOfMeasureId { get; set; }
    }
}
