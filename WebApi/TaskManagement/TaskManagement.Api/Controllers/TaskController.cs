using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="dto">Task details.</param>
        /// <returns>Created task.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var result = await _taskService.CreateAsync(dto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Updates a task.
        /// </summary>
        /// <param name="dto">Updated details.</param>
        /// <returns>Updated task.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTaskDto dto)
        {
            var result = await _taskService.UpdateAsync(dto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>Success status.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _taskService.DeleteAsync(id);
            return result.Success ? Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        /// Gets a task by ID.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <returns>Task details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _taskService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.Errors);
        }

        /// <summary>
        /// Gets filtered tasks with pagination.
        /// </summary>
        /// <param name="filter">Filter criteria.</param>
        /// <returns>Paged tasks.</returns>
        [HttpPost("filter")]
        public async Task<IActionResult> GetFiltered([FromBody] TaskFilterDto filter)
        {
            var result = await _taskService.GetFilteredAsync(filter);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Assigns a user to a task.
        /// </summary>
        /// <param name="taskId">Task ID.</param>
        /// <param name="userId">User ID.</param>
        /// <returns>Success status.</returns>
        [HttpPost("assign-user/{taskId}/{userId}")]
        public async Task<IActionResult> AssignUser(Guid taskId, Guid userId)
        {
            var result = await _taskService.AssignUserAsync(taskId, userId);
            return result.Success ? Ok() : BadRequest(result.Errors);
        }
    }
}