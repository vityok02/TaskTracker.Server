using Application.Modules.Tasks;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskModel, TaskDto>();
    }
}
