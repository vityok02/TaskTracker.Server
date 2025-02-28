using Api.Controllers.ProjectMember.Responses;
using Application.Modules.Members;
using AutoMapper;

namespace Api.Mapping;

public class ProjectMemberProfile : Profile
{
    public ProjectMemberProfile()
    {
        CreateMap<ProjectMemberDto, ProjectMemberResponse>();

        CreateMap<IEnumerable<ProjectMemberDto>, IEnumerable<ProjectMemberResponse>>();
    }
}
