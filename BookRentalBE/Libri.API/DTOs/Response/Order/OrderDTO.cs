namespace Libri.API.DTOs.Response.Order;

public class OrderDTO
{
    public int OrderId { get; set; }
    public string UserEmail { get; set; }
    public string OrderDate { get; set; }
    public int OrderTotal { get; set; }
    public string Status { get; set; }
    public string PaymentIntentId { get; set; }
    public string DeliveryShortName { get; set; }
    public string PaymentMethod { get; set; } = "COD";
}