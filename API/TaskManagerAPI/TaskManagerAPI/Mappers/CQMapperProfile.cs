using AutoMapper;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.Models.FE.APIRequests;

namespace TaskManagerAPI.Mappers
{
    public class CQMapperProfile : Profile
    {
        public CQMapperProfile()
        {
            CreateMap<LoginRequest, LoginCommand>();
            CreateMap<SignUpRequest, SignUpCommand>();
        }
    }
}
