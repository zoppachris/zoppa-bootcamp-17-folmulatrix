using AutoMapper;
using EF_Customer_Orders.DTOs.Orders;
using EF_Customer_Orders.Models;

namespace EF_Customer_Orders.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName,
                           opt => opt.MapFrom(src => src.Customer.CustName))
                .ForMember(dest => dest.AgentName,
                           opt => opt.MapFrom(src => src.Agent.AgentName));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Agent, opt => opt.Ignore());
        }
    }
}