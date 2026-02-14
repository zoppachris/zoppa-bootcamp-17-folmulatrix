using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ServiceResult<ProjectDto>> CreateAsync(CreateProjectDto dto, Guid ownerId);
        Task<ServiceResult<ProjectDto>> UpdateAsync(UpdateProjectDto dto);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<ServiceResult<ProjectDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<PagedResult<ProjectDto>>> GetAllAsync(int pageIndex, int pageSize);
        Task<ServiceResult> AssignMemberAsync(AssignMemberDto dto);
        Task<ServiceResult> RemoveMemberAsync(AssignMemberDto dto);
    }
}