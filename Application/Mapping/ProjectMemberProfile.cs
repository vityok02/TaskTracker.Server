using Application.Modules.Members;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping;

public sealed class ProjectMemberProfile : Profile
{
    public ProjectMemberProfile()
    {
        CreateMap<ProjectMemberModel, ProjectMemberDto>();
    }
}
