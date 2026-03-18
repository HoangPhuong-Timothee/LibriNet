namespace Libri.API.DTOs.Response.Order;

public class OrderItemDTO
{
    public int OrderItemId { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; }
    public string BookImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}