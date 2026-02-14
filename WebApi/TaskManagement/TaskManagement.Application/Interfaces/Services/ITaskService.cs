using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<ServiceResult<TaskDto>> CreateAsync(CreateTaskDto dto);
        Task<ServiceResult<TaskDto>> UpdateAsync(UpdateTaskDto dto);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<ServiceResult<TaskDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<PagedResult<TaskDto>>> GetFilteredAsync(TaskFilterDto filter);
        Task<ServiceResult> AssignUserAsync(Guid taskId, Guid userId);
    }
}