using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(LibriContext context) : base(context)
        {

        }
    }
}
