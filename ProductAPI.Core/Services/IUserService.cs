using Microsoft.AspNetCore.Identity;
using ProductAPI.Core.DTOs;
using ProductAPI.Core.DTOs.Authentication;

namespace ProductAPI.Core.Services
{
    public interface IUserService : IService<IdentityUser>
    {
        Task<UserDto> FindUserAsync(string userName, string password);
        Task<AuthResponseDto> LoginAsync(AuthRequestDto request);
        Task<IdentityUser> SignUpAsync(AuthRequestDto authDto);
        Task AddRoleAsync(string userId, string roleName);
    }
}