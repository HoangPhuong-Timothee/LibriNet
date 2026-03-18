using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomPublisher;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<IEnumerable<PublisherWithTotalCount>> GetAllPublishersForAdminAsync(PublisherParams publisherParam);
        Task<StoreProcedureResult> AddNewPublisherAsync(Publisher publisher);
        Task<StoreProcedureResult> ImportPublishersFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> UpdatePublisherAsync(int id, Publisher publisher);
        Task<StoreProcedureResult> DeletePublisherByIdAsync(int id);
        Task<bool> CheckPublisherExistByNameAsync(string name);
    }
}
