using Application.Projects.CreateProject;
using Application.Projects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectRequest, Project>();

        CreateMap<Project, ProjectResponse>();
    }
}
