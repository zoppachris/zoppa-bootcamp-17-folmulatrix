using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Helpers;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public TaskService(
            ITaskRepository taskRepo,
            IProjectRepository projectRepo,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _taskRepo = taskRepo;
            _projectRepo = projectRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        private async Task<bool> UserHasAccessToProjectAsync(Guid userId, Guid projectId)
        {
            var project = await _projectRepo.GetQueryable()
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return false;

            if (project.OwnerId == userId)
                return true;

            return project.Members.Any(m => m.UserId == userId);
        }

        private async Task<bool> IsAdminAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, "Admin");
        }

        private IQueryable<TaskItem> GetTaskQueryWithIncludes()
        {
            return _taskRepo.GetQueryable()
                .Include(t => t.Project)
                .Include(t => t.AssignedUser);
        }

        public async Task<ServiceResult<TaskItemDto>> CreateTaskAsync(Guid userId, CreateTaskDto createDto)
        {
            var hasAccess = await UserHasAccessToProjectAsync(userId, createDto.ProjectId);
            if (!hasAccess && !await IsAdminAsync(userId))
                return ServiceResult<TaskItemDto>.Fail("You do not have access to this project.", 403);

            if (createDto.AssignedUserId.HasValue)
            {
                var assignedUser = await _userManager.FindByIdAsync(createDto.AssignedUserId.Value.ToString());
                if (assignedUser == null)
                    return ServiceResult<TaskItemDto>.Fail("Assigned user not found.");

                var assignedHasAccess = await UserHasAccessToProjectAsync(createDto.AssignedUserId.Value, createDto.ProjectId);
                if (!assignedHasAccess && !await IsAdminAsync(createDto.AssignedUserId.Value))
                    return ServiceResult<TaskItemDto>.Fail("Assigned user does not have access to this project.");
            }

            var taskItem = _mapper.Map<TaskItem>(createDto);
            taskItem.CreatedAt = DateTime.UtcNow;

            var createdTask = await _taskRepo.AddAsync(taskItem);

            var taskWithIncludes = await GetTaskQueryWithIncludes()
                .FirstOrDefaultAsync(t => t.Id == createdTask.Id);

            var taskDto = _mapper.Map<TaskItemDto>(taskWithIncludes);
            return ServiceResult<TaskItemDto>.Ok(taskDto);
        }

        public async Task<ServiceResult<TaskItemDto>> UpdateTaskAsync(Guid userId, UpdateTaskDto updateDto)
        {
            var task = await GetTaskQueryWithIncludes()
                .FirstOrDefaultAsync(t => t.Id == updateDto.Id);
            if (task == null)
                return ServiceResult<TaskItemDto>.Fail("Task not found.");

            var hasAccess = await UserHasAccessToProjectAsync(userId, task.ProjectId);
            if (!hasAccess && !await IsAdminAsync(userId))
                return ServiceResult<TaskItemDto>.Fail("You do not have access to this task.", 403);

            if (updateDto.AssignedUserId.HasValue && updateDto.AssignedUserId != task.AssignedUserId)
            {
                var assignedUser = await _userManager.FindByIdAsync(updateDto.AssignedUserId.Value.ToString());
                if (assignedUser == null)
                    return ServiceResult<TaskItemDto>.Fail("Assigned user not found.");

                var assignedHasAccess = await UserHasAccessToProjectAsync(updateDto.AssignedUserId.Value, task.ProjectId);
                if (!assignedHasAccess && !await IsAdminAsync(updateDto.AssignedUserId.Value))
                    return ServiceResult<TaskItemDto>.Fail("Assigned user does not have access to this project.");
            }

            _mapper.Map(updateDto, task);

            await _taskRepo.UpdateAsync(task);

            var updatedTask = await GetTaskQueryWithIncludes()
                .FirstOrDefaultAsync(t => t.Id == task.Id);

            var taskDto = _mapper.Map<TaskItemDto>(updatedTask);
            return ServiceResult<TaskItemDto>.Ok(taskDto);
        }

        public async Task<ServiceResult<bool>> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null)
                return ServiceResult<bool>.Fail("Task not found.");

            var hasAccess = await UserHasAccessToProjectAsync(userId, task.ProjectId);
            if (!hasAccess && !await IsAdminAsync(userId))
                return ServiceResult<bool>.Fail("You do not have access to this task.", 403);

            await _taskRepo.DeleteAsync(task);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<TaskItemDto>> GetTaskByIdAsync(Guid userId, Guid taskId)
        {
            var task = await GetTaskQueryWithIncludes()
                .FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return ServiceResult<TaskItemDto>.Fail("Task not found.");

            var hasAccess = await UserHasAccessToProjectAsync(userId, task.ProjectId);
            if (!hasAccess && !await IsAdminAsync(userId))
                return ServiceResult<TaskItemDto>.Fail("You do not have access to this task.", 403);

            var taskDto = _mapper.Map<TaskItemDto>(task);
            return ServiceResult<TaskItemDto>.Ok(taskDto);
        }

        public async Task<ServiceResult<PagedResult<TaskItemDto>>> GetTasksAsync(Guid userId, TaskFilterDto filter)
        {
            var query = _taskRepo.GetQueryable()
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.Project.OwnerId == userId || t.Project.Members.Any(m => m.UserId == userId));

            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status.Value);
            if (filter.Priority.HasValue)
                query = query.Where(t => t.Priority == filter.Priority.Value);
            if (filter.ProjectId.HasValue)
                query = query.Where(t => t.ProjectId == filter.ProjectId.Value);
            if (filter.AssignedUserId.HasValue)
                query = query.Where(t => t.AssignedUserId == filter.AssignedUserId);
            if (filter.DueDateFrom.HasValue)
                query = query.Where(t => t.DueDate >= filter.DueDateFrom.Value);
            if (filter.DueDateTo.HasValue)
                query = query.Where(t => t.DueDate <= filter.DueDateTo.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(search) ||
                                         (t.Description != null && t.Description.ToLower().Contains(search)));
            }

            var allowedSortFields = new[] { "title", "status", "priority", "duedate", "createdat" };

            if (!string.IsNullOrWhiteSpace(filter.SortBy) && allowedSortFields.Contains(filter.SortBy.ToLower()))
            {
                query = query.ApplySorting(filter.SortBy, filter.SortOrder);
            }
            else
            {
                query = query.OrderBy(t => t.DueDate);
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var result = new PagedResult<TaskItemDto>
            {
                Items = items,
                TotalCount = total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
            return ServiceResult<PagedResult<TaskItemDto>>.Ok(result);
        }

        public async Task<ServiceResult<bool>> AssignTaskToUserAsync(Guid currentUserId, Guid taskId, Guid? assignUserId)
        {
            var task = await _taskRepo.GetQueryable()
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId);
                
            if (task == null)
                return ServiceResult<bool>.Fail("Task not found.");

            var hasAccess = await UserHasAccessToProjectAsync(currentUserId, task.ProjectId);
            if (!hasAccess && !await IsAdminAsync(currentUserId))
                return ServiceResult<bool>.Fail("You do not have permission to assign tasks in this project.", 403);

            if (!assignUserId.HasValue)
            {
                task.AssignedUserId = null;
                await _taskRepo.UpdateAsync(task);
                return ServiceResult<bool>.Ok(true);
            }

            var assignUser = await _userManager.FindByIdAsync(assignUserId.Value.ToString());
            if (assignUser == null)
                return ServiceResult<bool>.Fail("Assigned user not found.");

            var assignHasAccess = await UserHasAccessToProjectAsync(assignUserId.Value, task.ProjectId);
            if (!assignHasAccess && !await IsAdminAsync(assignUserId.Value))
                return ServiceResult<bool>.Fail("Assigned user does not have access to this project.");

            task.AssignedUserId = assignUserId;
            await _taskRepo.UpdateAsync(task);

            return ServiceResult<bool>.Ok(true);
        }
    }
}