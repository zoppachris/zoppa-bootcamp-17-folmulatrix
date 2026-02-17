using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Helpers;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IRepository<ProjectMember> _projectMemberRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IRepository<ProjectMember> projectMemberRepo, UserManager<AppUser> userManager, IMapper mapper)
        {
            _projectRepo = projectRepository;
            _projectMemberRepo = projectMemberRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ProjectDto>> CreateProjectAsync(Guid ownerId, CreateProjectDto createDto)
        {
            var owner = await _userManager.FindByIdAsync(ownerId.ToString());
            if (owner == null)
                return ServiceResult<ProjectDto>.Fail("User not found.");

            var project = _mapper.Map<Project>(createDto);
            project.OwnerId = ownerId;
            project.CreatedAt = DateTime.UtcNow;

            var created = await _projectRepo.AddAsync(project);

            var member = new ProjectMember { ProjectId = created.Id, UserId = ownerId };
            await _projectMemberRepo.AddAsync(member);

            var projectDto = _mapper.Map<ProjectDto>(created);

            return ServiceResult<ProjectDto>.Ok(projectDto);
        }

        public async Task<ServiceResult<ProjectDto>> UpdateProjectAsync(Guid userId, UpdateProjectDto updateDto)
        {
            var project = await _projectRepo.GetByIdAsync(updateDto.Id);
            if (project == null)
                return ServiceResult<ProjectDto>.Fail("Project not found.");

            if (project.OwnerId != userId)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");
                if (!isAdmin)
                    return ServiceResult<ProjectDto>.Fail("You are not authorized to update this project.");
            }

            _mapper.Map(updateDto, project);
            await _projectRepo.UpdateAsync(project);

            var projectDto = _mapper.Map<ProjectDto>(project);
            return ServiceResult<ProjectDto>.Ok(projectDto);
        }

        public async Task<ServiceResult<bool>> DeleteProjectAsync(Guid userId, Guid projectId)
        {
            var project = await _projectRepo.GetByIdAsync(projectId);
            if (project == null)
                return ServiceResult<bool>.Fail("Project not found.");

            if (project.OwnerId != userId)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");
                if (!isAdmin)
                    return ServiceResult<bool>.Fail("You are not authorized to delete this project.");
            }

            await _projectRepo.DeleteAsync(project);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<PagedResult<ProjectDto>>> GetUserProjectsAsync(Guid userId, ProjectFilterDto filter)
        {
            var query = _projectRepo.GetQueryable()
                .Include(p => p.Owner)
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .Where(p => p.OwnerId == userId || p.Members.Any(m => m.UserId == userId));

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(search) || (p.Description != null && p.Description.ToLower().Contains(search)));
            }

            var allowedSortFields = new[] { "name", "createdat" };

            if (!string.IsNullOrWhiteSpace(filter.SortBy) && allowedSortFields.Contains(filter.SortBy.ToLower()))
            {
                query = query.ApplySorting(filter.SortBy, filter.SortOrder);
            }
            else
            {
                query = query.OrderBy(p => p.CreatedAt);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var result = new PagedResult<ProjectDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
            return ServiceResult<PagedResult<ProjectDto>>.Ok(result);
        }

        public async Task<ServiceResult<ProjectDto>> GetProjectByIdAsync(Guid userId, Guid projectId)
        {
            var project = await _projectRepo.GetQueryable()
                .Include(p => p.Owner)
                .Include(p => p.Members).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return ServiceResult<ProjectDto>.Fail("Project not found.");

            if (project.OwnerId != userId && !project.Members.Any(m => m.UserId == userId))
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");
                if (!isAdmin)
                    return ServiceResult<ProjectDto>.Fail("You do not have access to this project.");
            }

            var dto = _mapper.Map<ProjectDto>(project);
            return ServiceResult<ProjectDto>.Ok(dto);
        }

        public async Task<ServiceResult<bool>> AssignMemberAsync(Guid currentUserId, AssignMemberDto assignDto)
        {
            var project = await _projectRepo.GetByIdAsync(assignDto.ProjectId);
            if (project == null)
                return ServiceResult<bool>.Fail("Project not found.");

            if (project.OwnerId != currentUserId)
                return ServiceResult<bool>.Fail("Only project owner can assign members.");

            var user = await _userManager.FindByIdAsync(assignDto.UserId.ToString());
            if (user == null)
                return ServiceResult<bool>.Fail("User not found.");

            var existing = await _projectMemberRepo.FindAsync(m => m.ProjectId == assignDto.ProjectId && m.UserId == assignDto.UserId);
            if (existing.Any())
                return ServiceResult<bool>.Fail("User is already a member.");

            var member = new ProjectMember { ProjectId = assignDto.ProjectId, UserId = assignDto.UserId };
            await _projectMemberRepo.AddAsync(member);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> RemoveMemberAsync(Guid currentUserId, Guid projectId, Guid memberId)
        {
            var project = await _projectRepo.GetByIdAsync(projectId);
            if (project == null)
                return ServiceResult<bool>.Fail("Project not found.");

            if (project.OwnerId != currentUserId)
                return ServiceResult<bool>.Fail("Only project owner can remove members.");

            var member = (await _projectMemberRepo.FindAsync(m => m.ProjectId == projectId && m.UserId == memberId)).FirstOrDefault();
            if (member == null)
                return ServiceResult<bool>.Fail("Member not found.");

            await _projectMemberRepo.DeleteAsync(member);
            return ServiceResult<bool>.Ok(true);
        }
    }
}