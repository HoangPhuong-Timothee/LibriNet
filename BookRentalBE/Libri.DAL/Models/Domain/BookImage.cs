namespace Libri.DAL.Models.Domain
{
    public partial class BookImage
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public bool IsPrimary { get; set; }
        public string PublicId { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Book Book { get; set; } = null!;
    }
}
