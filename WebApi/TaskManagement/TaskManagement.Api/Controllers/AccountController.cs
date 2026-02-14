using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Account;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="dto">Registration details.</param>
        /// <returns>Token if successful.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Logs in a user and returns JWT.
        /// </summary>
        /// <param name="dto">Login credentials.</param>
        /// <returns>Token if successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _accountService.LoginAsync(dto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Refreshes the JWT using refresh token.
        /// </summary>
        /// <param name="dto">Refresh token.</param>
        /// <returns>New token if valid.</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _accountService.RefreshTokenAsync(dto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Assigns a role to a user (Admin only).
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="role">Role (Admin/User).</param>
        /// <returns>Success status.</returns>
        [HttpPost("assign-role/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(Guid userId, [FromBody] string role)
        {
            var result = await _accountService.AssignRoleAsync(userId, role);
            return result.Success ? Ok() : BadRequest(result.Errors);
        }
    }
}