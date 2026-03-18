using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class DeliveryMethodRepository : GenericRepository<DeliveryMethod>, IDeliveryMethodRepository
    {
        public DeliveryMethodRepository(LibriContext context) : base(context)
        {

        }
    }
}
