using Application.Modules.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskDto>();

        CreateMap<TaskModel, TaskDto>();
    }
}
