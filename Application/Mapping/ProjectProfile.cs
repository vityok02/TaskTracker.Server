using Application.Modules.Projects;
using Application.Modules.Projects.CreateProject;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping;

public sealed class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectCommand, ProjectEntity>();

        CreateMap<ProjectStateModel, ProjectStateDto>();

        CreateMap<ProjectModel, ProjectDto>()
            .ForMember(dest => dest.States, opt => opt.MapFrom(src => src.States.AsEnumerable()));

        CreateMap<ProjectModel, ProjectEntity>();
    }
}
