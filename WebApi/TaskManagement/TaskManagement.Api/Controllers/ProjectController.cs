using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="dto">Project details.</param>
        /// <returns>Created project.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var ownerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());
            var result = await _projectService.CreateAsync(dto, ownerId);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Updates a project.
        /// </summary>
        /// <param name="dto">Updated details.</param>
        /// <returns>Updated project.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProjectDto dto)
        {
            var result = await _projectService.UpdateAsync(dto);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>Success status.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _projectService.DeleteAsync(id);
            return result.Success ? Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        /// Gets a project by ID.
        /// </summary>
        /// <param name="id">Project ID.</param>
        /// <returns>Project details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.Errors);
        }

        /// <summary>
        /// Gets all projects with pagination.
        /// </summary>
        /// <param name="pageIndex">Page index (1-based).</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged projects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _projectService.GetAllAsync(pageIndex, pageSize);
            return result.Success ? Ok(result.Data) : BadRequest(result.Errors);
        }

        /// <summary>
        /// Assigns a member to a project.
        /// </summary>
        /// <param name="dto">Assignment details.</param>
        /// <returns>Success status.</returns>
        [HttpPost("assign-member")]
        public async Task<IActionResult> AssignMember([FromBody] AssignMemberDto dto)
        {
            var result = await _projectService.AssignMemberAsync(dto);
            return result.Success ? Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        /// Removes a member from a project.
        /// </summary>
        /// <param name="dto">Removal details.</param>
        /// <returns>Success status.</returns>
        [HttpPost("remove-member")]
        public async Task<IActionResult> RemoveMember([FromBody] AssignMemberDto dto)
        {
            var result = await _projectService.RemoveMemberAsync(dto);
            return result.Success ? Ok() : BadRequest(result.Errors);
        }
    }
}