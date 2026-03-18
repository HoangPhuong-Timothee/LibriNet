namespace Libri.DAL.Models.Custom.CustomOrder
{
    public class ShippingAddress
    {
        public string FullName { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}