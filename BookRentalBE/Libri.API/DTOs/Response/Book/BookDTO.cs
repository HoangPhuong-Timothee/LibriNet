namespace Libri.API.DTOs.Response.Book
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Publisher { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int? PublishYear { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string CreateInfo { get; set; }
        public string UpdateInfo { get; set; }
        public bool? IsAvailable { get; set; }
        public string Isbn { get; set; }
    }
}
