using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomUnitOfMeasure;
using Libri.DAL.Models.Domain;
using Libri.DAL.Models.Xml;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IUnitOfMeasureService
    {
        Task<IEnumerable<UnitOfMeasureWithTotalCount>> GetAllUnitOfMeasuresForAdminAsync(UnitOfMeasureParams uomParams);
        Task<IEnumerable<UnitOfMeasure>> GetAllUnitOfMeasuresAsync();
        Task<StoreProcedureResult> AddNewUnitOfMeasureAsync(MeasureUnit uomXmlModel);
        Task<StoreProcedureResult> ImportUnitOfMeasuresFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> UpdateUnitOfMeasureAsync(int id, MeasureUnit uomXmlModel);
        Task<StoreProcedureResult> DeleteUnitOfMeasureByIdAsync(int id);
        Task<bool> CheckUnitOfMeasureExistByNameAsync(string name);
    }
}
