using TaskManagement.Application.DTOs.Account;
using TaskManagement.Application.Helpers;

namespace TaskManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ServiceResult<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
    }
}