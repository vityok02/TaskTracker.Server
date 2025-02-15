using Api.Controllers.Project;
using Application.Modules.Projects;
using AutoMapper;

namespace Api.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectDto, ProjectResponse>();
    }
}
