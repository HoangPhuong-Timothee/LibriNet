namespace Libri.DAL.Models.DataTable
{
    public class ExportInventoryDT
    {
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public string StoreName { get; set; }
        public DateTime ExportDate { get; set; }
        public string ExportNotes { get; set; }
        public int RowNumber { get; set; }
    }
}