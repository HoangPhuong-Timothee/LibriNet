namespace Libri.DAL.Models.DataTable
{
    public class ImportInventoryDT
    {
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public string StoreName { get; set; }
        public DateTime ImportDate { get; set; }
        public string ImportNotes { get; set; }
        public int RowNumber { get; set; }
    }
}
