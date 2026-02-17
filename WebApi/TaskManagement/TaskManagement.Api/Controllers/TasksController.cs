using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Validators.Task;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<TaskItemDto>>>> GetTasks([FromQuery] TaskFilterDto filter)
        {
            var validator = new TaskFilterDtoValidator();
            var validationResult = await validator.ValidateAsync(filter);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<TaskItemDto>.Fail("Filter Task failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _taskService.GetTasksAsync(UserId, filter);
            if (!result.Success)
                return BadRequest(ApiResponse<PagedResult<TaskItemDto>>.Fail(result.Message!));

            return Ok(ApiResponse<PagedResult<TaskItemDto>>.Ok(result.Data!));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> GetTask(Guid id)
        {
            var result = await _taskService.GetTaskByIdAsync(UserId, id);
            if (!result.Success)
                return NotFound(ApiResponse<TaskItemDto>.Fail(result.Message!));

            return Ok(ApiResponse<TaskItemDto>.Ok(result.Data!));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> CreateTask(CreateTaskDto createDto)
        {
            var validator = new CreateTaskDtoValidator();
            var validationResult = await validator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<TaskItemDto>.Fail("Create Task failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _taskService.CreateTaskAsync(UserId, createDto);
            if (!result.Success)
            {
                if (result.StatusCode == 403)
                    return Forbid();
                return BadRequest(ApiResponse<TaskItemDto>.Fail(result.Message!));
            }

            return CreatedAtAction(nameof(GetTask), new { id = result.Data!.Id }, ApiResponse<TaskItemDto>.Ok(result.Data!));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> UpdateTask(UpdateTaskDto updateDto)
        {
            var validator = new UpdateTaskDtoValidator();
            var validationResult = await validator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<TaskItemDto>.Fail("Update Task failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _taskService.UpdateTaskAsync(UserId, updateDto);
            if (!result.Success)
            {
                if (result.StatusCode == 403)
                    return Forbid();
                return BadRequest(ApiResponse<TaskItemDto>.Fail(result.Message!));
            }

            return Ok(ApiResponse<TaskItemDto>.Ok(result.Data!));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTask(Guid id)
        {
            var result = await _taskService.DeleteTaskAsync(UserId, id);
            if (!result.Success)
            {
                if (result.StatusCode == 403)
                    return Forbid();
                return BadRequest(ApiResponse<bool>.Fail(result.Message!));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Task deleted successfully"));
        }

        [HttpPost("{taskId:guid}/assign/{assignUserId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignTask(Guid taskId, Guid assignUserId)
        {
            var result = await _taskService.AssignTaskToUserAsync(UserId, taskId, assignUserId);
            if (!result.Success)
            {
                if (result.StatusCode == 403)
                    return Forbid();
                return BadRequest(ApiResponse<bool>.Fail(result.Message!));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Task assigned successfully"));
        }

        [HttpPost("{taskId:guid}/unassign")]
        public async Task<ActionResult<ApiResponse<bool>>> UnassignTask(Guid taskId)
        {
            var result = await _taskService.AssignTaskToUserAsync(UserId, taskId, null);
            if (!result.Success)
            {
                if (result.StatusCode == 403)
                    return Forbid();
                return BadRequest(ApiResponse<bool>.Fail(result.Message!));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Task unassigned successfully"));
        }
    }
}