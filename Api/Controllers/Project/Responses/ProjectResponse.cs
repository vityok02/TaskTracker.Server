﻿using Api.Controllers.Abstract;
using Api.Controllers.Role.Responses;
using Api.Controllers.State.Responses;

namespace Api.Controllers.Project.Responses;

public class ProjectResponse : AuditableResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public IEnumerable<StateResponse> States { get; init; } = [];

    public RoleResponse? Role { get; init; }
}
