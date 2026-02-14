using AutoMapper;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepo, IUserRepository userRepo, IMapper mapper)
        {
            _projectRepo = projectRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ProjectDto>> CreateAsync(CreateProjectDto dto, Guid ownerId)
        {
            var project = _mapper.Map<Project>(dto);
            project.OwnerId = ownerId;
            await _projectRepo.AddAsync(project);
            await _projectRepo.SaveChangesAsync();
            return ServiceResult<ProjectDto>.Successful(_mapper.Map<ProjectDto>(project));
        }

        public async Task<ServiceResult<ProjectDto>> UpdateAsync(UpdateProjectDto dto)
        {
            var project = await _projectRepo.GetByIdAsync(dto.Id);
            if (project == null) return ServiceResult<ProjectDto>.Failed("Project not found");

            _mapper.Map(dto, project);
            _projectRepo.Update(project);
            await _projectRepo.SaveChangesAsync();
            return ServiceResult<ProjectDto>.Successful(_mapper.Map<ProjectDto>(project));
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            if (project == null) return ServiceResult.Failed("Project not found");

            _projectRepo.Delete(project);
            await _projectRepo.SaveChangesAsync();
            return ServiceResult.Successful();
        }

        public async Task<ServiceResult<ProjectDto>> GetByIdAsync(Guid id)
        {
            var project = await _projectRepo.GetProjectWithMembersAsync(id); // Include members
            if (project == null) return ServiceResult<ProjectDto>.Failed("Project not found");

            return ServiceResult<ProjectDto>.Successful(_mapper.Map<ProjectDto>(project));
        }

        public async Task<ServiceResult<PagedResult<ProjectDto>>> GetAllAsync(int pageIndex, int pageSize)
        {
            var query = _projectRepo.GetAll().OrderBy(p => p.CreatedAt);
            var (items, total) = await _projectRepo.GetPagedAsync(query, pageIndex, pageSize);
            var dtos = _mapper.Map<IEnumerable<ProjectDto>>(items);

            var paged = new PagedResult<ProjectDto> { Items = dtos, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
            return ServiceResult<PagedResult<ProjectDto>>.Successful(paged);
        }

        public async Task<ServiceResult> AssignMemberAsync(AssignMemberDto dto)
        {
            if (await _userRepo.GetByIdAsync(dto.UserId) == null) return ServiceResult.Failed("User not found");
            await _projectRepo.AssignMemberAsync(dto.ProjectId, dto.UserId);
            return ServiceResult.Successful();
        }

        public async Task<ServiceResult> RemoveMemberAsync(AssignMemberDto dto)
        {
            await _projectRepo.RemoveMemberAsync(dto.ProjectId, dto.UserId);
            return ServiceResult.Successful();
        }
    }
}