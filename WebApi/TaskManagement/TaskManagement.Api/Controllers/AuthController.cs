using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Account;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Validators.Account;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator();
            var validationResult = await validator.ValidateAsync(registerDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<AuthResponseDto>.Fail("Validation failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
                return BadRequest(ApiResponse<AuthResponseDto>.Fail(result.Message!));

            return Ok(ApiResponse<AuthResponseDto>.Ok(result.Data!));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto loginDto)
        {
            var validator = new LoginDtoValidator();
            var validationResult = await validator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<AuthResponseDto>.Fail("Login failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
                return Unauthorized(ApiResponse<AuthResponseDto>.Fail(result.Message!));

            return Ok(ApiResponse<AuthResponseDto>.Ok(result.Data!));
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<bool>>> Logout(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.LogoutAsync(refreshTokenDto.RefreshToken);

            if (!result.Success)
                return Unauthorized(ApiResponse<bool>.Fail(result.Message!));

            return Ok(ApiResponse<bool>.Ok(true, "Logged out successfully"));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var validator = new RefreshTokenDtoValidator();
            var validationResult = await validator.ValidateAsync(refreshTokenDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<AuthResponseDto>.Fail("Refresh Token failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (!result.Success)
                return BadRequest(ApiResponse<AuthResponseDto>.Fail(result.Message!));

            return Ok(ApiResponse<AuthResponseDto>.Ok(result.Data!));
        }
    }
}