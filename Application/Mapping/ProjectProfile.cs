using AutoMapper;
using Domain.Entities;
using Application.Modules.Projects;
using Application.Modules.Projects.CreateProject;

namespace Application.Mapping;

public sealed class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectRequest, Project>();

        CreateMap<Project, ProjectResponse>();
    }
}
