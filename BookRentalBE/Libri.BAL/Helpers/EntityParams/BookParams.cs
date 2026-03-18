namespace Libri.BAL.Helpers.EntityParams
{
    public class BookParams : Params
    {
        public int? AuthorId { get; set; }
        public int? GenreId { get; set; }
        public int? PublisherId { get; set; }
    }
}