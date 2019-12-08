using AutoMapper;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.FE.TasksDtos;

namespace TaskManagerAPI.Models.Mappers
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<TaskDomain, TaskToGetDto>();
            CreateMap<TaskToBeCreatedDto, TaskDomain>();
            CreateMap<TaskForFullUpdatedDto, TaskDomain>().ReverseMap();
            CreateMap<TaskForFullUpdatedDto, TaskForUpdated>();
            CreateMap<TaskForUpdated, TaskDomain>();
        }
    }
}
