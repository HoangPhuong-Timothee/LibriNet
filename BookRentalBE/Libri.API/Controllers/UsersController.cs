using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Errors;
using Libri.API.DTOs.Response.User;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.Pagination;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile = Libri.DAL.Models.Xml.Profile;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper, IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet("admin/users-list")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Pagination<MemeberDTO>>> GetAllUsersAsync([FromQuery] UserParams userParams)
        {
            var result = await _userService.GetUsersListAsync(userParams);

            var members = _mapper.Map<IEnumerable<MemeberDTO>>(result);

            var totalUsers = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<MemeberDTO>(userParams.PageIndex, userParams.PageSize, totalUsers, members);

            return Ok(response);

        }

        [HttpGet("current-user")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync()
        {
            var result = await _userService.GetUserByIdAsync();

            var currentUser = result.FirstOrDefault();

            var response = _mapper.Map<UserDTO>(currentUser);

            response.Token = _tokenService.GenerateAccessToken(result);

            foreach (var user in result)
            {
                response.Roles.Add(user.Role);
            }

            return Ok(response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> ModifyUserProfileAsync([FromBody] ModifyProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userProfile = _mapper.Map<Profile>(request);
            
            var updateUserProfile = await _userService.UpdateUserProfileAsync(userProfile);

            if (!updateUserProfile.Success)
            {
                return BadRequest(new APIResponse(400, updateUserProfile.Message));
            }

            return Ok(new { message = updateUserProfile.Message });
        }

        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetAddressByUserIdAsync()
        {
            var userAddress = await _userService.GetAddressByUserIdAsync();
            
            var response = _mapper.Map<AddressDTO>(userAddress);
            
            return Ok(response);
        }

        [HttpPut("address")]
        public async Task<ActionResult> ModifyUserAddressAsync([FromBody] ModifyAddressRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = _mapper.Map<Address>(request); 
            
            var modifyUserAddress = await _userService.UpdateUserAddressAsync(address);
            
            if (!modifyUserAddress.Success) 
            {
                return BadRequest(new APIResponse(400, modifyUserAddress.Message));
            }

            return Ok(new { message = modifyUserAddress.Message });
        }

        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadUserAvatarAsync([FromForm] IFormFile file)
        {
            var uploadUserAvatar = await _userService.UploadUserImageAsync(file);

            if (!uploadUserAvatar.Success)
            {
                return BadRequest(new APIResponse(400, uploadUserAvatar.Message));
            }

            return StatusCode(201, new { message = uploadUserAvatar.Message });
        }
    }
}