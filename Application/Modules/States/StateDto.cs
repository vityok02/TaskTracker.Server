﻿using Application.Abstract;
using Domain.Constants;

namespace Application.Modules.States;

public class StateDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string Color { get; init; } = DefaultColor.Value;

    public int SortOrder { get; init; }
}
