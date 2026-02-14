using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Account;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountService(IUserRepository userRepo, IMapper mapper, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ServiceResult<TokenDto>> RegisterAsync(RegisterDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var result = await _userRepo.CreateUserAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return ServiceResult<TokenDto>.Failed(result.Errors.Select(e => e.Description).ToArray());
            }

            await _userRepo.AddToRoleAsync(user, dto.Role);
            return ServiceResult<TokenDto>.Successful(await _tokenService.GenerateTokensAsync(user));
        }

        public async Task<ServiceResult<TokenDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.FindByEmailAsync(dto.Email);
            if (user == null || !await _userRepo.CheckPasswordAsync(user, dto.Password))
            {
                return ServiceResult<TokenDto>.Failed("Invalid email or password");
            }

            return ServiceResult<TokenDto>.Successful(await _tokenService.GenerateTokensAsync(user));
        }

        public async Task<ServiceResult<TokenDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            try
            {
                var tokens = await _tokenService.RefreshTokenAsync(dto.RefreshToken);
                return ServiceResult<TokenDto>.Successful(tokens);
            }
            catch (Exception ex)
            {
                return ServiceResult<TokenDto>.Failed(ex.Message);
            }
        }

        public async Task<ServiceResult> AssignRoleAsync(Guid userId, string role)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return ServiceResult.Failed("User not found");

            var result = await _userRepo.AddToRoleAsync(user, role);
            return result.Succeeded ? ServiceResult.Successful() : ServiceResult.Failed(result.Errors.Select(e => e.Description).ToArray());
        }
    }
}