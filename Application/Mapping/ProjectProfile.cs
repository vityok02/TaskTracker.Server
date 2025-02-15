using Application.Modules.Projects.CreateProject;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public sealed class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectCommand, Project>();
    }
}
