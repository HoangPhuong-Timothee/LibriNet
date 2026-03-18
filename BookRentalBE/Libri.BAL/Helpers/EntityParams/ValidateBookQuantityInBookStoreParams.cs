namespace Libri.BAL.Helpers.EntityParams
{
    public class ValidateBookQuantityInBookStoreParams
    {
        public int BookStoreId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int InputQuantity { get; set; }
        public string BookTitle { get; set; }
        public string Isbn { get; set; }
    }
}
