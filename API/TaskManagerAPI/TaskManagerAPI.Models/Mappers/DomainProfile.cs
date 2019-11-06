using AutoMapper;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.FE.TasksDtos;

namespace TaskManagerAPI.Models.Mappers
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Task, TaskToGetDto>();
            CreateMap<TaskToBeCreatedDto, Task>();
            CreateMap<TaskForFullUpdatedDto, Task>().ReverseMap();
            CreateMap<TaskForFullUpdatedDto, TaskForUpdated>();
            CreateMap<TaskForUpdated, Task>();
        }
    }
}
