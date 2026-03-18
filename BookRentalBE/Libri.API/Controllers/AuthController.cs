using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Errors;
using Libri.API.DTOs.Response.User;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Xml;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(IMapper mapper, IAuthService authService, ITokenService tokenService)
        {
            _mapper = mapper;
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerParams = _mapper.Map<Register>(request);
            
            var registerAccount = await _authService.Register(registerParams);
            
            if (!registerAccount.Success)
            {
                return BadRequest(new APIResponse(400, registerAccount.Message));
            }
           
            return Ok(new { message = registerAccount.Message });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var login = await _authService.Login(request.Email, request.Password);

            if (!login.Success)
            {
                return Unauthorized(new APIResponse(401, login.Message));
            }
            
            var loginUser = login.Data.FirstOrDefault();

            var response = _mapper.Map<UserDTO>(loginUser);
            
            response.Token = _tokenService.GenerateAccessToken(login.Data);

            foreach(var user in login.Data)
            {
                response.Roles.Add(user.Role);
            }

            return Ok(response);
        }

        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> CheckEmailIsExistAsync([FromQuery] string email)
        {
            return await _authService.CheckEmailExistAsync(email);
        }
    }
}
