using Libri.DAL.Models.Custom;

namespace Libri.BAL.Helpers;

public class ServiceResponse<T> : StoreProcedureResult
{
    public T Data { get; set; }

}