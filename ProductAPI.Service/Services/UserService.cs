using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProductAPI.Core.DTOs;
using ProductAPI.Core.DTOs.Authentication;
using ProductAPI.Core.Services;
using ProductAPI.Core.UnitOfWorks;
using ProductAPI.Service.Authorization.Abstraction;
using System.Linq.Expressions;

namespace ProductAPI.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IMapper mapper, IJwtAuthenticationManager jwtAuthenticationManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> LoginAsync(AuthRequestDto request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

            if (!result.Succeeded)
            {
                return null; // Giriş başarısızsa, null döndürüyoruz 
            }

            // Giriş başarılı, JWT token oluşturuluyor.
            var user = await _userManager.FindByNameAsync(request.UserName);
            var token = _jwtAuthenticationManager.Authenticate(user.UserName, request.Password);

            return new AuthResponseDto
            {
                Token = token.Token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<IdentityUser> SignUpAsync(AuthRequestDto authDto)
        {
            var user = new IdentityUser
            {
                UserName = authDto.UserName,
                Email = authDto.Email
            };

            var result = await _userManager.CreateAsync(user, authDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed.");
            }

            return user;
        }

        public async Task<UserDto> FindUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
            {
                return null; // Kullanıcı bulunamazsa veya şifre yanlışsa.
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IdentityUser> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }

        public IQueryable<IdentityUser> Where(Expression<Func<IdentityUser, bool>> expression)
        {
            return _userManager.Users.Where(expression);
        }

        public async Task<IdentityUser> AddAsync(IdentityUser entity)
        {
            var result = await _userManager.CreateAsync(entity);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to add user.");
            }
            return entity;
        }

        public async Task UpdateAsync(IdentityUser entity)
        {
            var result = await _userManager.UpdateAsync(entity);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user.");
            }
        }

        public async Task RemoveAsync(IdentityUser entity)
        {
            var result = await _userManager.DeleteAsync(entity);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to remove user.");
            }
        }

        public async Task AddRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Kullanıcının zaten bu role sahip olup olmadığını kontrol et
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                throw new Exception($"User is already in role: {roleName}");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to assign role");
            }
        }

    }
}


