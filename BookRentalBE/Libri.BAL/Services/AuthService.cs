using Dapper;
using Libri.BAL.Helpers;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomUser;
using Libri.DAL.Models.Domain;
using Libri.DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Libri.DAL.Models.Xml;
using Libri.BAL.Extensions;
using Libri.DAL.Models.Custom;

namespace Libri.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        public AuthService(ILogger<AuthService> logger, IUnitOfWork uow, ITokenService tokenService, IUserService userService)
        {
            _uow = uow;
            _tokenService = tokenService;
            _logger = logger;
            _userService = userService;
        }

        public async Task<StoreProcedureResult> Register(Register register)
        {
            var response = new StoreProcedureResult();

            bool existEmail = await CheckEmailExistAsync(register.Email);

            if (existEmail)
            {
                response.Success = false;
                response.Message = "Email đã được đăng ký.";
                return response;
            }

            string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt(10);
            string passwordHashed = BCrypt.Net.BCrypt.HashPassword(register.Password + passwordSalt);
            register.PasswordSalt = passwordSalt;
            register.Password = passwordHashed;

            string registerXml = register.SingleObjectToXml();

            var parameters = new DynamicParameters();
            parameters.Add("@RegisterXml", registerXml, dbType: DbType.Xml);

            string spName = "sp_RegisterUser";

            try
            {
                var result = await _uow.Dappers
                        .ExecuteStoreProcedureReturnAsync<StoreProcedureResult>(spName, parameters);

                return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi đăng ký thông tin: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi trong quá trình đăng ký tài khoản", e);
            }
        }

        public async Task<ServiceResponse<IEnumerable<CurrentUser>>> Login(string email, string password)
        {
            var response = new ServiceResponse<IEnumerable<CurrentUser>>();

            var userAuth = await _uow.UserAuths
                    .Queryable()
                    .Select(ua => new UserAuth
                    {
                        UserInfo = ua.UserInfo,
                        Email = ua.Email,
                        Password = ua.Password,
                        PasswordSalt = ua.PasswordSalt
                    })
                    .Where(ua => EF.Functions.Collate(email, "Latin1_General_BIN") == EF.Functions.Collate(ua.Email, "Latin1_General_BIN"))
                    .FirstOrDefaultAsync();

            if (userAuth == null)
            {
                _logger.LogError("{email} không đúng hoặc chưa đăng ký.", email);
                response.Message = "Email không đúng hoặc chưa đăng ký.";
                response.Success = false;
                return response;
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(password + userAuth!.PasswordSalt, userAuth!.Password);

            if (!validPassword)
            {
                _logger.LogError("Mật khẩu không đúng");
                response.Message = "Mật khẩu hoặc email không đúng.";
                response.Success = false;
                return response;
            }

            var user = await _userService.GetUserByEmailAsync(email);
            
            userAuth.LastLoggedIn = DateTime.Now;
            _uow.UserAuths.Update(userAuth);
            await _uow.CommitAsync();

            response.Message = "Đăng nhập thành công";
            response.Data = user;

            return response;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            try
            {
                return await _uow.UserAuths
                    .Queryable()
                    .Where(ua => EF.Functions.Collate(email, "Latin1_General_BIN") == EF.Functions.Collate(ua.Email, "Latin1_General_BIN"))
                    .AnyAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Lỗi tìm kiếm người dùng theo email: {Exception}", e);

                if (e.InnerException != null)
                {
                    _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
                }

                throw new Exception("Có lỗi xảy ra trong quá trình kiểm tra email", e);
            }
        }
    }
}
