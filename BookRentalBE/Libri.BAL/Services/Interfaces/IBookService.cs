using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomBook;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookWithDetailsAndTotalCount>> GetAllBooksAsync(BookParams bookParams);
        Task<IEnumerable<BookWithDetails>> GetLatestBooksAsync();
        Task<IEnumerable<BookWithDetails>> GetSimilarBooksAsync(int bookId);
        Task<BookWithDetails> GetBookByIdAsync(int id);
        Task<StoreProcedureResult> ImportBooksFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> DeleteBookAsync(int id);
        Task<StoreProcedureResult> UploadBookImagesAsync(int bookId, List<IFormFile> files);
        Task<bool> CheckBookExistByTitleAsync(string bookTitle);
        Task<bool> CheckBookISBNAsync(string isbn, string bookTitle);
        Task<bool> CheckBookExistInBookStoreAsync(string bookTitle, int bookStoreId);
    }
}
