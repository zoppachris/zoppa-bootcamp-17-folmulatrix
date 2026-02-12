

using EF_Customer_Orders.DTOs.Orders;

namespace EF_Customer_Orders.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task DeleteAsync(Guid id);
    }
}