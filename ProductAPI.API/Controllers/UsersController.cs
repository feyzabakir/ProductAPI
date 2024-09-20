using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Core.DTOs;
using ProductAPI.Core.DTOs.Authentication;
using ProductAPI.Core.Services;

namespace ProductAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBasesController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IMapper mapper, IUserService userService, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var users = await _userService.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users.ToList());
            return CreateActionResult(GlobalResultDto<List<UserDto>>.Success(200, userDtos));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null)
            {
                return CreateActionResult(GlobalResultDto<NoContentDto>.Fail(404, "User not found"));
            }

            user.Email = userDto.Email;
            user.UserName = userDto.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return CreateActionResult(GlobalResultDto<NoContentDto>.Fail(400, "Failed to update user"));
            }

            return CreateActionResult(GlobalResultDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return CreateActionResult(GlobalResultDto<NoContentDto>.Fail(404, "User not found"));
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return CreateActionResult(GlobalResultDto<NoContentDto>.Fail(400, "Failed to delete user"));
            }

            return CreateActionResult(GlobalResultDto<NoContentDto>.Success(204));
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp([FromBody] AuthRequestDto authDto)
        {
            var user = await _userService.SignUpAsync(authDto);
            var userDto = _mapper.Map<UserDto>(user);
            return CreateActionResult(GlobalResultDto<UserDto>.Success(201, userDto));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthRequestDto authDto)
        {
            var result = await _userService.LoginAsync(authDto);
            if (result.User != null)
            {
                return CreateActionResult(GlobalResultDto<AuthResponseDto>.Success(200, result));
            }

            return CreateActionResult(GlobalResultDto<AuthResponseDto>.Fail(401, "Invalid login attempt"));
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("{id}/AddRole")]
        public async Task<IActionResult> AddRole(string id, [FromQuery] string roleName)
        {
            try
            {
                await _userService.AddRoleAsync(id, roleName);
                return Ok(new { message = $"Role '{roleName}' assigned to user with ID: {id}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
