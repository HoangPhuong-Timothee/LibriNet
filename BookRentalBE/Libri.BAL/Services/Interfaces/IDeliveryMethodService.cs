using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomDeliveryMethod;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IDeliveryMethodService
    {
        Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync();
        Task<IEnumerable<DeliveryMethodWithTotalCount>> GetAllDeliveryMethodsForAdminAsync(DeliveryMethodParams dmParams);
        Task<StoreProcedureResult> ImportDeliveryMethodsFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> DeleteDeliveryMethodByIdAsync(int dmId);
        Task<DeliveryMethod?> GetDeliveryMethodByIdAsync(int id);
    }
}
