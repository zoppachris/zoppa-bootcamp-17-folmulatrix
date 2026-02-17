using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Validators.Project;

namespace TaskManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue("userId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProjectDto>>>> GetProjects([FromQuery] ProjectFilterDto filter)
        {
            var result = await _projectService.GetUserProjectsAsync(UserId, filter);
            return Ok(ApiResponse<PagedResult<ProjectDto>>.Ok(result.Data!));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(Guid id)
        {
            var result = await _projectService.GetProjectByIdAsync(UserId, id);
            if (!result.Success)
                return NotFound(ApiResponse<ProjectDto>.Fail(result.Message!));

            return Ok(ApiResponse<ProjectDto>.Ok(result.Data!));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject(CreateProjectDto createDto)
        {
            var validator = new CreateProjectDtoValidator();
            var validationResult = await validator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<ProjectDto>.Fail("Create Project failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _projectService.CreateProjectAsync(UserId, createDto);
            if (!result.Success)
                return BadRequest(ApiResponse<ProjectDto>.Fail(result.Message!));

            return CreatedAtAction(nameof(GetProject), new { id = result.Data!.Id }, ApiResponse<ProjectDto>.Ok(result.Data!));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(UpdateProjectDto updateDto)
        {
            var validator = new UpdateProjectDtoValidator();
            var validationResult = await validator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<ProjectDto>.Fail("Update Project failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _projectService.UpdateProjectAsync(UserId, updateDto);
            if (!result.Success)
                return BadRequest(ApiResponse<ProjectDto>.Fail(result.Message!));

            return Ok(ApiResponse<ProjectDto>.Ok(result.Data!));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProject(Guid id)
        {
            var result = await _projectService.DeleteProjectAsync(UserId, id);
            if (!result.Success)
                return BadRequest(ApiResponse<bool>.Fail(result.Message!));

            return Ok(ApiResponse<bool>.Ok(true, "Project deleted successfully"));
        }

        [HttpPost("assign-member")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignMember(AssignMemberDto assignDto)
        {
            var validator = new AssignMemberDtoValidator();
            var validationResult = await validator.ValidateAsync(assignDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<ProjectDto>.Fail("Assign Member failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var result = await _projectService.AssignMemberAsync(UserId, assignDto);
            if (!result.Success)
                return BadRequest(ApiResponse<bool>.Fail(result.Message!));

            return Ok(ApiResponse<bool>.Ok(true, "Member assigned successfully"));
        }

        [HttpDelete("{projectId:guid}/members/{memberId:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveMember(Guid projectId, Guid memberId)
        {
            var result = await _projectService.RemoveMemberAsync(UserId, projectId, memberId);
            if (!result.Success)
                return BadRequest(ApiResponse<bool>.Fail(result.Message!));

            return Ok(ApiResponse<bool>.Ok(true, "Member removed successfully"));
        }
    }
}