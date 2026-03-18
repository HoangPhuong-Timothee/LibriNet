using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class UserAddressRepository : GenericRepository<Address>, IUserAddressRepository
    {
        public UserAddressRepository(LibriContext context) : base(context)
        {
            
        }
    }
}
