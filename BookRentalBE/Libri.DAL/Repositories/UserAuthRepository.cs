using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class UserAuthRepository : GenericRepository<UserAuth>, IUserAuthRepository
    {
        public UserAuthRepository(LibriContext context) : base(context)
        {

        }
    }
}
