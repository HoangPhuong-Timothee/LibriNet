namespace Libri.API.DTOs.Response.Inventory
{
    public class BookStoreDTO
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateInfo { get; set; }
        public int? TotalQuantity { get; set; }
    }
}
