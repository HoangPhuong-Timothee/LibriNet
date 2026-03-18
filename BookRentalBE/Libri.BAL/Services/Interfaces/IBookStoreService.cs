using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomBookStore;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IBookStoreService
    {
        Task<IEnumerable<BookStoreWithTotalCount>> GetAllBookStoresForAdminAsync(BookStoreParams bookStoreParams);
        Task<IEnumerable<BookStore>> GetAllBookStoresAsync();
        Task<StoreProcedureResult> AddNewBookStoreAsync(BookStore bookStore);
        Task<StoreProcedureResult> ImportBookStoresFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> UpdateBookStoreAsync(int id, BookStore bookStore);
        Task<StoreProcedureResult> DeleteBookStoreByIdAsync(int id);
        Task<bool> CheckBookStoreExistByStoreNameAsync(string storeName);
    }
}
