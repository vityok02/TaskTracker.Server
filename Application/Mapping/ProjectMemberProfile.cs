using Application.Modules.Members;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public sealed class ProjectMemberProfile : Profile
{
    public ProjectMemberProfile()
    {
        CreateMap<ProjectMember, MemberResponse>();
    }
}
