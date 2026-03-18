using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Response.Basket
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; } //Id of the book in basket
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
