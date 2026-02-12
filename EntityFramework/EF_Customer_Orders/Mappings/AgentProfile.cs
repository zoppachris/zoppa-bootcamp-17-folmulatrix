using AutoMapper;
using EF_Customer_Orders.DTOs.Agents;
using EF_Customer_Orders.Models;

namespace EF_Customer_Orders.Mappings
{
    public class AgentProfile : Profile
    {
        public AgentProfile()
        {
            CreateMap<Agent, AgentDto>();

            CreateMap<CreateAgentDto, Agent>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(_ => Guid.NewGuid()));

            CreateMap<UpdateAgentDto, Agent>()
                .ForMember(dest => dest.AgentCode, opt => opt.Ignore())
                .ForMember(dest => dest.Customers, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore());
        }
    }
}