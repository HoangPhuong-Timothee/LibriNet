namespace Libri.API.DTOs.Response.Order
{
    public class DeliveryMethodDTO
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateInfo { get; set; }
    }
}
