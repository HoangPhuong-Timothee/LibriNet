namespace Libri.DAL.Models.Custom.CustomBookStore
{
    public class BookStoreWithTotalCount
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalQuantity { get; set; }
        public string ISBN { get; set; }
        public int TotalCount { get; set; }
    }
}
