using practice_webapi.Application.DTOs;
using practice_webapi.Application.Helpers;

namespace practice_webapi.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    }
}