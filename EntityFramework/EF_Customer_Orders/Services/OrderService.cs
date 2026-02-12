using AutoMapper;
using EF_Customer_Orders.Data;
using EF_Customer_Orders.DTOs.Orders;
using EF_Customer_Orders.Models;
using EF_Customer_Orders.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF_Customer_Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly CustomerOrdersDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(CustomerOrdersDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            List<Order> orders = await _context.Orders
                            .AsNoTracking()
                            .Include(c => c.Agent)
                            .Include(c => c.Customer)
                            .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }
        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            Order? order = await _context.Orders
                            .AsNoTracking()
                            .Include(c => c.Agent)
                            .Include(c => c.Customer)
                            .FirstOrDefaultAsync(a => a.Id == id);

            if (order == null)
                return null;

            return _mapper.Map<OrderDto>(order);
        }
        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            bool ordNumExists = await _context.Orders
            .AnyAsync(o => o.OrdNum == dto.OrdNum);

            if (ordNumExists)
                throw new InvalidOperationException("OrdNum already exists.");

            bool agentExists = await _context.Agents
                .AnyAsync(a => a.Id == dto.AgentId);

            if (!agentExists)
                throw new InvalidOperationException("Agent not found.");

            bool customerExists = await _context.Customers
                .AnyAsync(a => a.Id == dto.CustomerId);

            if (!customerExists)
                throw new InvalidOperationException($"Customer not found.");

            bool validRelation = await _context.Customers
                .AnyAsync(c => c.Id == dto.CustomerId && c.AgentId == dto.AgentId);

            if (!validRelation)
                throw new InvalidOperationException(
                    "Customer does not belong to the specified agent.");

            Order order = _mapper.Map<Order>(dto);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Agent)
                .FirstAsync(o => o.Id == order.Id);

            return _mapper.Map<OrderDto>(order);
        }
        public async Task DeleteAsync(Guid id)
        {
            Order? order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}