namespace Libri.DAL.Models.Custom.CustomPublisher
{
    public class PublisherWithTotalCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { set; get; }
        public int TotalCount { get; set; }
    }
}
