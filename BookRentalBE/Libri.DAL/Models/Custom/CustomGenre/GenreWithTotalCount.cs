namespace Libri.DAL.Models.Custom.CustomGenre
{
    public class GenreWithTotalCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { set; get; }
        public int TotalCount { get; set; }
    }
}
