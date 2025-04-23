using Api.Controllers.Project.Responses;
using Application.Modules.Projects;
using AutoMapper;
using Domain.Abstract;

namespace Api.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectDto, ProjectResponse>();

        CreateMap<PagedList<ProjectDto>, PagedList<ProjectResponse>>();
    }
}
