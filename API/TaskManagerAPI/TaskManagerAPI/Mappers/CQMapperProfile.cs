using AutoMapper;
using TaskManagerAPI.CQRS.AuthProcess.Commands;
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
