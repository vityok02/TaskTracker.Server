using Application.Modules.Projects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public sealed class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectResponse>();
    }
}
