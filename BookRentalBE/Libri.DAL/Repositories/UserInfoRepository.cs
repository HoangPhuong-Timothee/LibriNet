using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class UserInfoRepository : GenericRepository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(LibriContext context) : base(context)
        {

        }
    }
}
