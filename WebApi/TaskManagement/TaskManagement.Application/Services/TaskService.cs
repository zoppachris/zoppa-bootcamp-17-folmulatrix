using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepo, IProjectRepository projectRepo, IUserRepository userRepo, IMapper mapper)
        {
            _taskRepo = taskRepo;
            _projectRepo = projectRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TaskDto>> CreateAsync(CreateTaskDto dto)
        {
            if (await _projectRepo.GetByIdAsync(dto.ProjectId) == null) return ServiceResult<TaskDto>.Failed("Project not found");
            if (dto.AssignedToId.HasValue && await _userRepo.GetByIdAsync(dto.AssignedToId.Value) == null) return ServiceResult<TaskDto>.Failed("User not found");

            var task = _mapper.Map<TaskItem>(dto);
            await _taskRepo.AddAsync(task);
            await _taskRepo.SaveChangesAsync();
            return ServiceResult<TaskDto>.Successful(_mapper.Map<TaskDto>(task));
        }

        public async Task<ServiceResult<TaskDto>> UpdateAsync(UpdateTaskDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(dto.Id);
            if (task == null) return ServiceResult<TaskDto>.Failed("Task not found");
            if (dto.AssignedToId.HasValue && await _userRepo.GetByIdAsync(dto.AssignedToId.Value) == null) return ServiceResult<TaskDto>.Failed("User not found");

            _mapper.Map(dto, task);
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
            return ServiceResult<TaskDto>.Successful(_mapper.Map<TaskDto>(task));
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) return ServiceResult.Failed("Task not found");

            _taskRepo.Delete(task);
            await _taskRepo.SaveChangesAsync();
            return ServiceResult.Successful();
        }

        public async Task<ServiceResult<TaskDto>> GetByIdAsync(Guid id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) return ServiceResult<TaskDto>.Failed("Task not found");

            return ServiceResult<TaskDto>.Successful(_mapper.Map<TaskDto>(task));
        }

        public async Task<ServiceResult<PagedResult<TaskDto>>> GetFilteredAsync(TaskFilterDto filter)
        {
            var query = _taskRepo.GetAll().AsQueryable();

            if (filter.Status.HasValue) query = query.Where(t => t.Status == filter.Status.Value);
            if (filter.ProjectId.HasValue) query = query.Where(t => t.ProjectId == filter.ProjectId.Value);
            if (filter.AssignedUserId.HasValue) query = query.Where(t => t.AssignedToId == filter.AssignedUserId.Value);
            if (filter.DueDate.HasValue) query = query.Where(t => t.DueDate == filter.DueDate.Value);

            query = query.OrderBy(t => t.DueDate ?? DateTime.MaxValue); // Sort by due date

            var (items, total) = await _taskRepo.GetPagedAsync(query, filter.PageIndex, filter.PageSize);
            var dtos = _mapper.Map<IEnumerable<TaskDto>>(items);

            var paged = new PagedResult<TaskDto> { Items = dtos, TotalCount = total, PageIndex = filter.PageIndex, PageSize = filter.PageSize };
            return ServiceResult<PagedResult<TaskDto>>.Successful(paged);
        }

        public async Task<ServiceResult> AssignUserAsync(Guid taskId, Guid userId)
        {
            if (await _userRepo.GetByIdAsync(userId) == null) return ServiceResult.Failed("User not found");
            await _taskRepo.AssignUserAsync(taskId, userId);
            return ServiceResult.Successful();
        }
    }
}