using Api.Controllers.Task.Responses;
using Application.Modules.Tasks;
using AutoMapper;

namespace Api.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskDto, TaskResponse>();
    }
}
