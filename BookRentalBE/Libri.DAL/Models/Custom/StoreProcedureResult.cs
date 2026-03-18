using Libri.DAL.Models.Custom.CustomError;

namespace Libri.DAL.Models.Custom
{
    public class StoreProcedureResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public List<ErrorDetails>? Errors { get; set; }
    }
}