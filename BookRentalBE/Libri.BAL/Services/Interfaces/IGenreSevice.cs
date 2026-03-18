using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomGenre;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IGenreSevice
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<IEnumerable<GenreWithTotalCount>> GetAllGenresForAdminAsync(GenreParams genreParams);
        Task<StoreProcedureResult> AddNewGenreAsync(Genre genre);
        Task<StoreProcedureResult> ImportGenresFromFileAsync(IFormFile file);
        Task<StoreProcedureResult> UpdateGenreAsync(int id, Genre genre);
        Task<StoreProcedureResult> DeleteGenreByIdAsync(int id);
        Task<bool> CheckGenreExistByNameAsync(string name);
    }
}
