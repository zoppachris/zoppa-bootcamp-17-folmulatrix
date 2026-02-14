using TaskManagement.Application.DTOs.Account;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokensAsync(User user);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
    }
}