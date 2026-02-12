
using EF_Customer_Orders.DTOs.Customers;

namespace EF_Customer_Orders.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(Guid id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto);
        Task DeleteAsync(Guid id);
    }
}