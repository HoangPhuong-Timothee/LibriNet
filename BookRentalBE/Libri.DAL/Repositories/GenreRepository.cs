using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(LibriContext context) : base(context)
        {

        }
    }
}
