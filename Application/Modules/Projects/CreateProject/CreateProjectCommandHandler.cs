﻿using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Modules.States;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Projects.CreateProject;

internal sealed class CreateProjectCommandHandler
    : ICommandHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDateTimeProvider _dateTimeService;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;

    public CreateProjectCommandHandler(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IDateTimeProvider dateTimeService,
        IMapper mapper,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _dateTimeService = dateTimeService;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }

    public async Task<Result<ProjectDto>> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        // TODO: improve db queries
        // Implement ProjectService
        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result<ProjectDto>
                .Failure(UserErrors.NotFound);
        }

        bool projectExists = await _projectRepository
            .ExistsByNameAsync(command.UserId, command.Name);

        if (projectExists)
        {
            return Result<ProjectDto>
                .Failure(ProjectErrors.AlreadyExists);
        }

        var role = await _roleRepository
            .GetByNameAsync(Domain.Constants.Roles.Admin);

        if (role is null)
        {
            return Result<ProjectDto>
                .Failure(RoleErrors.NotFound);
        }

        var project = _mapper.Map<ProjectEntity>(command);

        project.Id = Guid.NewGuid();
        project.CreatedAt = _dateTimeService
            .GetCurrentTime();
        project.CreatedBy = command.UserId;

        var projectId = await _projectRepository
            .CreateAsync(project, role.Id);

        var states = ProjectDefaults
            .GetDefaultStates(projectId, user.Id, _dateTimeService.GetCurrentTime());

        var projectDto = new ProjectDto
        {
            Id = projectId,
            Name = project.Name,
            Description = project.Description,
            CreatedBy = user.Id,
            CreatedAt = project.CreatedAt,
            StartDate = project.StartDate
                ?? _dateTimeService.GetCurrentTime(),
            States = ProjectDefaults
                .GetDefaultStates(projectId, user.Id, _dateTimeService.GetCurrentTime())
                .Select(s => new StateDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Color = s.Color,
                    Description = s.Description,
                    SortOrder = s.SortOrder,
                })
        };

        return Result<ProjectDto>
            .Success(projectDto);
    }
}
