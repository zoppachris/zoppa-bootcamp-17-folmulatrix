using System.Security.Claims;
using practice_webapi.Domain.Entities;

namespace practice_webapi.Infrastructure.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(AppUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}