using Libri.BAL.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.DAL.Models.Custom;
using Libri.DAL.Models.Custom.CustomUser;
using Libri.DAL.Models.Domain;
using Libri.DAL.Models.Xml;
using Microsoft.AspNetCore.Http;

namespace Libri.BAL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<MemberWithTotalCount>> GetUsersListAsync(UserParams userParams);
        Task<IEnumerable<CurrentUser>> GetUserByIdAsync();
        Task<IEnumerable<CurrentUser>> GetUserByEmailAsync(string email);
        Task<Address?> GetAddressByUserIdAsync();
        Task<StoreProcedureResult> UpdateUserProfileAsync(Profile profile);
        Task<StoreProcedureResult> UpdateUserAddressAsync(Address address);
        Task<StoreProcedureResult> UploadUserImageAsync(IFormFile file);
        string GetUserEmailFromClaims();
        string GetUserNameFromClaims();
        int GetUserIdFromClaims();
    }
}
