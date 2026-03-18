using System.ComponentModel.DataAnnotations;
using Libri.API.DTOs.Response.User;

namespace Libri.API.DTOs.Request;

public class CreateOrderRequest
{
    [Required]
    public AddressDTO ShippingAddress { get; set; }       
    
    [Required]
    public int DeliveryMethodId { get; set; }
}