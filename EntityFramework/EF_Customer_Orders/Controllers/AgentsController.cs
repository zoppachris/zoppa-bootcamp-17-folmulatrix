using EF_Customer_Orders.DTOs.Agents;
using EF_Customer_Orders.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EF_Customer_Orders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentsController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        // GET: api/agents
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AgentDto>>> GetAll()
        {
            List<AgentDto> agents = await _agentService.GetAllAsync();
            return Ok(agents);
        }

        // GET: api/agents/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentDto>> GetById(Guid id)
        {
            AgentDto? agent = await _agentService.GetByIdAsync(id);

            if (agent == null)
                return NotFound();

            return Ok(agent);
        }

        // POST: api/agents
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AgentDto>> Create(
        [FromBody] CreateAgentDto dto)
        {
            try
            {
                AgentDto createdAgent = await _agentService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdAgent.Id },
                    createdAgent);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/agents/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentDto>> Update(
        Guid id,
        [FromBody] UpdateAgentDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id in route and body must match.");

            try
            {
                AgentDto updatedAgent = await _agentService.UpdateAsync(dto);
                return Ok(updatedAgent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/agents/{id}
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                bool deleted = await _agentService.DeleteAsync(id);

                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}