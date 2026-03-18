using Libri.DAL.Models.Custom.CustomUser;

namespace Libri.BAL.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<CurrentUser> users);
    }
}