using Microsoft.AspNetCore.Mvc;
using practice_webapi.Application.DTOs;
using practice_webapi.Application.Interfaces;
using practice_webapi.Application.Validators;

namespace practice_webapi.Controllers
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
            var validator = new RegisterValidator();
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
            var validator = new LoginValidator();
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
    }
}