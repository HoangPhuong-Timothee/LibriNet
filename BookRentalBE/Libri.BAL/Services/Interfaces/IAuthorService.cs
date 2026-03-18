using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomAuthor;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync(string searchTerm);
        Task<IEnumerable<AuthorWithTotalCount>> GetAllAuthorsForAdminAsync(AuthorParams authorParams);
        Task<StoreProcedureResult> AddNewAuthorAsync(Author author);
        Task<StoreProcedureResult> ImportAuthorsFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> UpdateAuthorAsync(int id, Author author);
        Task<StoreProcedureResult> DeleteAuthorByIdAsync(int id);
        Task<bool> CheckAuthorExistByNameAsync(string name);
    }
}
