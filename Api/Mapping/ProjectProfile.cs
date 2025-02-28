using Api.Controllers.Project.Responses;
using Application.Modules.Projects;
using AutoMapper;

namespace Api.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectStateDto, ProjectStateResponse>();

        CreateMap<ProjectDto, ProjectResponse>();
    }
}
