using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Members.GetAllMembers;

internal sealed class GetAllMembersQueryHandler
    : IQueryHandler<GetAllMembersQuery, IEnumerable<ProjectMemberDto>>
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetAllMembersQueryHandler(
        IProjectMemberRepository memberRepository,
        IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProjectMemberDto>>> Handle(
        GetAllMembersQuery query,
        CancellationToken cancellationToken)
    {
        var members = await _memberRepository
            .GetAllExtendedAsync(query.ProjectId);

        return Result<IEnumerable<ProjectMemberDto>>
            .Success(_mapper.Map<IEnumerable<ProjectMemberDto>>(members));
    }
}
