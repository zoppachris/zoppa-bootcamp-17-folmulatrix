
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using practice_webapi.Application.DTOs;
using practice_webapi.Application.Helpers;
using practice_webapi.Application.Interfaces;
using practice_webapi.Domain.Entities;
using practice_webapi.Infrastructure.Services;

namespace practice_webapi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var existingUserByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUserByEmail != null)
            {
                return ServiceResult<AuthResponseDto>.Fail("Email is already registered.");
            }

            var existingUserByUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserByUserName != null)
            {
                return ServiceResult<AuthResponseDto>.Fail("Username is already taken.");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return ServiceResult<AuthResponseDto>.Fail(errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _tokenService.GenerateTokenAsync(user);

            var userResult = _mapper.Map<UserDto>(user);

            return ServiceResult<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                User = userResult,
                Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"]))
            });
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return ServiceResult<AuthResponseDto>.Fail("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return ServiceResult<AuthResponseDto>.Fail("Invalid email or password.");

            var token = await _tokenService.GenerateTokenAsync(user);

            var userResult = _mapper.Map<UserDto>(user);

            var roles = await _userManager.GetRolesAsync(user);
            userResult.Roles = roles.ToList();

            return ServiceResult<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                User = userResult,
                Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"]))
            });
        }
    }
}