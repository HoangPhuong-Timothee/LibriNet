using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Response.Basket
{
    public class BasketDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public List<BasketItemDTO> BasketItems { get; set; }
        public decimal DeliveryPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
