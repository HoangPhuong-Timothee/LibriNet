namespace Libri.DAL.Models.Domain
{
    public partial class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string District { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }

        public virtual UserInfo User { get; set; } = null!;
    }
}