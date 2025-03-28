using Api.Controllers.Project.Responses;
using Application.Modules.Projects;
using AutoMapper;
using Domain.Abstract;
using Domain.Entities;
using Domain.Models;

namespace Api.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectStateDto, ProjectStateResponse>();

        CreateMap<ProjectDto, ProjectResponse>();

        CreateMap<PagedList<ProjectDto>, PagedList<ProjectResponse>>();
    }
}
