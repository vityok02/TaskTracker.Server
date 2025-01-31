using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Abstract;

namespace Application.Modules.Roles.GetAllRoles;

internal sealed class GetAllRolesQueryHandler
    : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleResponse>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<RoleResponse>>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync();

        return roles
            .Select(_mapper.Map<RoleResponse>)
            .ToArray();
    }
}
