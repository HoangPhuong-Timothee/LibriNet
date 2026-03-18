namespace Libri.DAL.Models.Custom.CustomBasket
{
    public class Basket
    {
        public Basket()
        {

        }

        public Basket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public decimal DeliveryPrice { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ClientSerect { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
