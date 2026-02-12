using AutoMapper;
using EF_Customer_Orders.DTOs.Customers;
using EF_Customer_Orders.Models;

namespace EF_Customer_Orders.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();

            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Agent, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.CustCode, opt => opt.Ignore())
                .ForMember(dest => dest.Agent, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());
        }
    }
}