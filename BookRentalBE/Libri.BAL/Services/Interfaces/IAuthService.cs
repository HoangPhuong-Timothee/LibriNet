using Libri.BAL.Helpers;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomUser;
using Libri.DAL.Models.Xml;

namespace Libri.BAL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<StoreProcedureResult> Register(Register register);
        Task<ServiceResponse<IEnumerable<CurrentUser>>> Login(string email, string password);
        Task<bool> CheckEmailExistAsync(string email);
    }
}
