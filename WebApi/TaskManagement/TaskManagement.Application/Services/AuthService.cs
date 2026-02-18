

using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TaskManagement.Application.DTOs.Account;
using TaskManagement.Application.Helpers;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Interfaces;
using TaskManagement.Infrastructure.Settings;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;
        private readonly IRepository<RefreshToken> _refreshTokenRepo;
        private readonly RefreshTokenSettings _refreshTokenSettings;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOptions<JwtSettings> jwtSettings, ITokenService tokenService,
        IRepository<RefreshToken> refreshTokenRepo,
        IOptions<RefreshTokenSettings> refreshTokenSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _tokenService = tokenService;
            _refreshTokenRepo = refreshTokenRepo;
            _refreshTokenSettings = refreshTokenSettings.Value;
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
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

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult<AuthResponseDto>.Fail(errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiryDays),
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.AddAsync(refreshTokenEntity);

            var token = await _tokenService.GenerateTokenAsync(user);
            return ServiceResult<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
            });
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return ServiceResult<AuthResponseDto>.Fail("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return ServiceResult<AuthResponseDto>.Fail("Invalid email or password.");

            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiryDays),
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.AddAsync(refreshTokenEntity);

            var token = await _tokenService.GenerateTokenAsync(user);
            return ServiceResult<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
            });
        }

        public async Task<ServiceResult<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntities = await _refreshTokenRepo.FindAsync(rt =>
                rt.Token == refreshToken &&
                !rt.IsRevoked &&
                rt.ExpiryDate > DateTime.UtcNow);

            var tokenEntity = tokenEntities.FirstOrDefault();
            if (tokenEntity == null)
                return ServiceResult<AuthResponseDto>.Fail("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(tokenEntity.UserId.ToString());
            if (user == null)
                return ServiceResult<AuthResponseDto>.Fail("User not found.");

            tokenEntity.IsRevoked = true;
            await _refreshTokenRepo.UpdateAsync(tokenEntity);

            await CleanupExpiredTokensForUserAsync(user.Id);

            var newAccessToken = await _tokenService.GenerateTokenAsync(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var newTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiryDays),
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.AddAsync(newTokenEntity);

            return ServiceResult<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
            });
        }

        public async Task<ServiceResult<bool>> LogoutAsync(string refreshToken)
        {
            var tokens = await _refreshTokenRepo.FindAsync(rt => rt.Token == refreshToken);

            var token = tokens.FirstOrDefault();

            if (token == null)
                return ServiceResult<bool>.Fail("Invalid or expired refresh token.");

            if (token != null)
            {
                token.IsRevoked = true;
                await _refreshTokenRepo.UpdateAsync(token);
                // await _refreshTokenRepo.DeleteAsync(token);
            }

            return ServiceResult<bool>.Ok(true, "Logged out successfully");
        }

        private async Task CleanupExpiredTokensForUserAsync(Guid userId)
        {
            var expiredTokens = await _refreshTokenRepo.FindAsync(rt =>
                rt.UserId == userId && rt.ExpiryDate < DateTime.UtcNow);

            foreach (var token in expiredTokens)
            {
                await _refreshTokenRepo.DeleteAsync(token);
            }
        }
    }
}