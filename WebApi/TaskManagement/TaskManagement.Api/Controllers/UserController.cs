using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.User;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetCurrentUser()
        {
            var result = await _userService.GetCurrentUserAsync(UserId);
            if (!result.Success)
                return NotFound(ApiResponse<UserProfileDto>.Fail(result.Message!));

            return Ok(ApiResponse<UserProfileDto>.Ok(result.Data!));
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<UserPublicDto>>>> GetUsers([FromQuery] UserFilterDto filter)
        {
            var result = await _userService.GetUsersAsync(filter);
            return Ok(ApiResponse<PagedResult<UserPublicDto>>.Ok(result.Data!));
        }
        [HttpPut("me")]

        public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateCurrentUser(UpdateUserDto updateDto)
        {
            var result = await _userService.UpdateUserAsync(UserId, updateDto);
            if (!result.Success)
                return BadRequest(ApiResponse<UserProfileDto>.Fail(result.Message!));

            return Ok(ApiResponse<UserProfileDto>.Ok(result.Data!));
        }
    }
}