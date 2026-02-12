using EF_Customer_Orders.DTOs.Agents;

namespace EF_Customer_Orders.Services.Interfaces
{
    public interface IAgentService
    {
        Task<List<AgentDto>> GetAllAsync();
        Task<AgentDto?> GetByIdAsync(Guid id);
        Task<AgentDto> CreateAsync(CreateAgentDto dto);
        Task<AgentDto> UpdateAsync(Guid id, UpdateAgentDto dto);
        Task DeleteAsync(Guid id);
    }
}