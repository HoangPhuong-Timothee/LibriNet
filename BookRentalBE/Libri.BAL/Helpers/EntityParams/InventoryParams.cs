namespace Libri.BAL.Helpers.EntityParams
{
    public class InventoryParams : Params
    {
        public int? GenreId { get; set; }
        public int? BookStoreId { get; set; }
        public string? InventoryStatus { get; set; }
        private string? _isbnSearch;
        public string IsbnSearch
        {
            get => _isbnSearch ?? "";
            set => _isbnSearch = value.ToLower();
        }
    }
}
