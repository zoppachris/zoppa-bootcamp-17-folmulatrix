using AutoMapper;
using EF_Customer_Orders.Data;
using EF_Customer_Orders.DTOs.Agents;
using EF_Customer_Orders.Models;
using EF_Customer_Orders.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF_Customer_Orders.Services
{
    public class AgentService : IAgentService
    {
        private readonly CustomerOrdersDbContext _context;
        private readonly IMapper _mapper;

        public AgentService(CustomerOrdersDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AgentDto>> GetAllAsync()
        {
            List<Agent> agents = await _context.Agents
                .AsNoTracking()
                .OrderBy(a => a.AgentCode)
                .ToListAsync();

            return _mapper.Map<List<AgentDto>>(agents);
        }
        public async Task<AgentDto?> GetByIdAsync(Guid id)
        {
            Agent? agent = await _context.Agents
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
                return null;

            return _mapper.Map<AgentDto>(agent);
        }
        public async Task<AgentDto> CreateAsync(CreateAgentDto dto)
        {
            bool agentCodeExists = await _context.Agents
                .AnyAsync(a => a.AgentCode == dto.AgentCode);

            if (agentCodeExists)
                throw new InvalidOperationException(
                    $"Agent with code '{dto.AgentCode}' already exists.");

            Agent agent = _mapper.Map<Agent>(dto);

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();

            return _mapper.Map<AgentDto>(agent);
        }
        public async Task<AgentDto> UpdateAsync(Guid id, UpdateAgentDto dto)
        {
            Agent? agent = await _context.Agents
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
                throw new KeyNotFoundException("Agent not found.");

            _mapper.Map(dto, agent);

            await _context.SaveChangesAsync();

            return _mapper.Map<AgentDto>(agent);
        }
        public async Task DeleteAsync(Guid id)
        {
            Agent? agent = await _context.Agents
                .Include(a => a.Customers)
                .Include(a => a.Orders)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
                throw new KeyNotFoundException("Agent not found.");

            bool hasCustomer = await _context.Customers.AnyAsync(c => c.AgentId == id);

            bool hasOrder = await _context.Orders
                .AnyAsync(o => o.AgentId == id);

            if (hasCustomer || hasOrder)
                throw new InvalidOperationException("Cannot delete agent because it is still referenced by customers or orders.");

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
        }
    }
}