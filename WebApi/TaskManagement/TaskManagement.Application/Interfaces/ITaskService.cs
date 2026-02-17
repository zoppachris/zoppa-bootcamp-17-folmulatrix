using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Helpers;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<ServiceResult<TaskItemDto>> CreateTaskAsync(Guid userId, CreateTaskDto createDto);
        Task<ServiceResult<TaskItemDto>> UpdateTaskAsync(Guid userId, UpdateTaskDto updateDto);
        Task<ServiceResult<bool>> DeleteTaskAsync(Guid userId, Guid taskId);
        Task<ServiceResult<TaskItemDto>> GetTaskByIdAsync(Guid userId, Guid taskId);
        Task<ServiceResult<PagedResult<TaskItemDto>>> GetTasksAsync(Guid userId, TaskFilterDto filter);
        Task<ServiceResult<bool>> AssignTaskToUserAsync(Guid currentUserId, Guid taskId, Guid? assignUserId);
    }
}