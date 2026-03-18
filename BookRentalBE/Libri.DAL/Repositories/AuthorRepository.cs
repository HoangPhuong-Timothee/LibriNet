using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibriContext context) : base(context)
        {

        }
    }
}
