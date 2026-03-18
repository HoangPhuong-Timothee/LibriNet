using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class BookStoreRepository : GenericRepository<BookStore>, IBookStoreRepository
    {
        public BookStoreRepository(LibriContext context) : base(context)
        {

        }
    }
}
