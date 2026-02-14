using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Account;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ServiceResult<TokenDto>> RegisterAsync(RegisterDto dto);
        Task<ServiceResult<TokenDto>> LoginAsync(LoginDto dto);
        Task<ServiceResult<TokenDto>> RefreshTokenAsync(RefreshTokenDto dto);
        Task<ServiceResult> AssignRoleAsync(Guid userId, string role);
    }
}