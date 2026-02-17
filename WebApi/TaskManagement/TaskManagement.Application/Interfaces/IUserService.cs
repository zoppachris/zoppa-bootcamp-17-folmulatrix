using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.User;
using TaskManagement.Application.Helpers;

namespace TaskManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserProfileDto>> GetCurrentUserAsync(Guid userId);
        Task<ServiceResult<PagedResult<UserPublicDto>>> GetUsersAsync(UserFilterDto filter);
        Task<ServiceResult<UserProfileDto>> UpdateUserAsync(Guid userId, UpdateUserDto updateDto);
    }
}