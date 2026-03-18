namespace Libri.BAL.Helpers
{
    public class FileValidation
    {
        public List<string> RequiredColumns { get; set; }
        public Dictionary<string, string> ColumnDataTypes { get; set; }
        public Dictionary<string, (Func<string, bool> Rule, string Description)> Rules { get; set; }

        public FileValidation()
        {
            RequiredColumns = new List<string>();
            ColumnDataTypes = new Dictionary<string, string>();
            Rules = new Dictionary<string, (Func<string, bool> Rule, string Description)>();
        }
    }
}
