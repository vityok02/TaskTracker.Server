using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Members.GetMember;

internal sealed class GetMemberQueryHandler
    : IQueryHandler<GetMemberQuery, ProjectMemberDto>
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetMemberQueryHandler(IProjectMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProjectMemberDto>> Handle(
        GetMemberQuery query,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository
            .GetExtendedAsync(query.UserId, query.ProjectId);

        if (member is null)
        {
            return Result<ProjectMemberDto>
                .Failure(ProjectMemberErrors.NotFound);
        }

        return _mapper.Map<ProjectMemberDto>(member);
    }
}
