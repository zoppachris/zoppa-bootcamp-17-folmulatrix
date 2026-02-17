using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.User;
using TaskManagement.Application.Helpers;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UserProfileDto>> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return ServiceResult<UserProfileDto>.Fail("User not found", 404);

            var userDto = _mapper.Map<UserProfileDto>(user);

            return ServiceResult<UserProfileDto>.Ok(userDto);
        }
        public async Task<ServiceResult<PagedResult<UserPublicDto>>> GetUsersAsync(UserFilterDto filter)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(u => u.UserName!.ToLower().Contains(search) || u.FullName.ToLower().Contains(search));
            }

            var allowedSortFields = new[] { "fullname", "username" };

            if (!string.IsNullOrWhiteSpace(filter.SortBy) && allowedSortFields.Contains(filter.SortBy.ToLower()))
            {
                query = query.ApplySorting(filter.SortBy, filter.SortOrder);
            }
            else
            {
                query = query.OrderBy(t => t.UserName);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ProjectTo<UserPublicDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var result = new PagedResult<UserPublicDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            return ServiceResult<PagedResult<UserPublicDto>>.Ok(result);
        }
        public async Task<ServiceResult<UserProfileDto>> UpdateUserAsync(Guid userId, UpdateUserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ServiceResult<UserProfileDto>.Fail("User not found", 404);

            if (!string.Equals(user.UserName, updateDto.UserName, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userManager.FindByNameAsync(updateDto.UserName);
                if (existingUser != null && existingUser.Id != userId)
                    return ServiceResult<UserProfileDto>.Fail("Username is already taken");
            }

            if (!string.Equals(user.Email, updateDto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingEmail = await _userManager.FindByEmailAsync(updateDto.Email);
                if (existingEmail != null && existingEmail.Id != userId)
                    return ServiceResult<UserProfileDto>.Fail("Email is already registered");
            }

            user.UserName = updateDto.UserName;
            user.Email = updateDto.Email;
            user.FullName = updateDto.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult<UserProfileDto>.Fail(errors);
            }

            var userDto = _mapper.Map<UserProfileDto>(user);

            return ServiceResult<UserProfileDto>.Ok(userDto);
        }
    }
}