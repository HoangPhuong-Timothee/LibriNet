namespace Libri.API.DTOs.Response.Book
{
    public class BookImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }
    }
}
