namespace Libri.DAL.Models.Custom.CustomBook
{
    public class BookWithDetailsAndTotalCount
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? Genre { get; set; }
        public string? Publisher { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int? PublishYear { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsAvailable { get; set; }
        public int TotalCount { get; set; }
        public string Isbn { get; set; }
    }
}
