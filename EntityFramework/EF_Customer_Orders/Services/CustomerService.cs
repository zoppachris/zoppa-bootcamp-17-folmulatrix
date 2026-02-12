using AutoMapper;
using EF_Customer_Orders.Data;
using EF_Customer_Orders.DTOs.Customers;
using EF_Customer_Orders.Models;
using EF_Customer_Orders.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF_Customer_Orders.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerOrdersDbContext _context;
        private readonly IMapper _mapper;

        public CustomerService(CustomerOrdersDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            List<Customer> customers = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Agent)
                .ToListAsync();

            return _mapper.Map<List<CustomerDto>>(customers);
        }
        public async Task<CustomerDto?> GetByIdAsync(Guid id)
        {
            Customer? customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Agent)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (customer == null)
                return null;

            return _mapper.Map<CustomerDto>(customer);
        }
        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            bool agentExists = await _context.Agents
            .AnyAsync(a => a.Id == dto.AgentId);

            if (!agentExists)
                throw new InvalidOperationException("Agent not found.");

            bool customerCodeExists = await _context.Customers
                .AnyAsync(a => a.CustCode == dto.CustCode);

            if (customerCodeExists)
                throw new InvalidOperationException(
                    $"Customer with code '{dto.CustCode}' already exists.");

            Customer customer = _mapper.Map<Customer>(dto);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Agent)
                .FirstAsync(c => c.Id == customer.Id);

            return _mapper.Map<CustomerDto>(customer);
        }
        public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto)
        {
            Customer? customer = await _context.Customers
               .FirstOrDefaultAsync(a => a.Id == id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            _mapper.Map(dto, customer);

            await _context.SaveChangesAsync();

            customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Agent)
                .FirstAsync(c => c.Id == customer.Id);

            return _mapper.Map<CustomerDto>(customer);
        }
        public async Task DeleteAsync(Guid id)
        {
            Customer? customer = await _context.Customers
               .FirstOrDefaultAsync(a => a.Id == id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            bool hasOrder = await _context.Orders
                .AnyAsync(o => o.CustomerId == id);

            if (hasOrder)
                throw new InvalidOperationException(
                    "Cannot delete customer because it is still referenced by orders.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}