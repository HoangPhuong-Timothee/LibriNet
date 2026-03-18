namespace Libri.DAL.Models.Custom.CustomBasket
{
    public class BasketItem
    {
        public int Id { get; set; } //Id of the book in the basket
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
