using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Helpers;

namespace TaskManagement.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult<ProjectDto>> CreateProjectAsync(Guid ownerId, CreateProjectDto createDto);
        Task<ServiceResult<ProjectDto>> UpdateProjectAsync(Guid userId, UpdateProjectDto updateDto);
        Task<ServiceResult<bool>> DeleteProjectAsync(Guid userId, Guid projectId);
        Task<ServiceResult<PagedResult<ProjectDto>>> GetUserProjectsAsync(Guid userId, ProjectFilterDto filter);
        Task<ServiceResult<ProjectDto>> GetProjectByIdAsync(Guid userId, Guid projectId);
        Task<ServiceResult<bool>> AssignMemberAsync(Guid currentUserId, AssignMemberDto assignDto);
        Task<ServiceResult<bool>> RemoveMemberAsync(Guid currentUserId, Guid projectId, Guid memberId);
    }
}