namespace Libri.DAL.Models.Xml
{
    [Serializable]
    public class Image
    {
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }
    }
}
