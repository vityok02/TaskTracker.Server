using Application.Abstract.Interfaces;
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
    private readonly ITemplateRepository _templateRepository;
    private readonly IDateTimeProvider _dateTimeService;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;
    private readonly IStateRepository _stateRepository;
    private readonly ITagRepository _tagRepository;

    public CreateProjectCommandHandler(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        ITemplateRepository templateRepository,
        IDateTimeProvider dateTimeService,
        IMapper mapper,
        IRoleRepository roleRepository,
        IStateRepository stateRepository,
        ITagRepository tagRepository)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _templateRepository = templateRepository;
        _dateTimeService = dateTimeService;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _stateRepository = stateRepository;
        _tagRepository = tagRepository;
    }

    public async Task<Result<ProjectDto>> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var getUserTask = _userRepository.GetByIdAsync(command.UserId);
        var checkProjectExistsTask = _projectRepository.ExistsByNameAsync(command.UserId, command.Name);
        var getRoleTask = _roleRepository.GetByNameAsync(Domain.Constants.Roles.Admin);

        await Task.WhenAll(getUserTask, checkProjectExistsTask, getRoleTask);

        var user = getUserTask.Result;
        var projectExists = checkProjectExistsTask.Result;
        var role = getRoleTask.Result;

        if (user is null)
        {
            return Result<ProjectDto>
                .Failure(UserErrors.NotFound);
        }

        if (projectExists)
        {
            return Result<ProjectDto>
                .Failure(ProjectErrors.AlreadyExists);
        }

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

        IEnumerable<StateEntity> states = [];

        if (command.TemplateId is not null && command.TemplateId != Guid.Empty)
        {
            var templateResult = await CreateTemplateAsync(
                projectId,
                command.TemplateId.Value,
                command.UserId);

            if (templateResult.IsFailure)
            {
                return Result<ProjectDto>
                    .Failure(templateResult.Error);
            }

            states = templateResult.Value;
        }

        var projectDto = new ProjectDto
        {
            Id = projectId,
            Name = project.Name,
            Description = project.Description,
            CreatedBy = user.Id,
            CreatedAt = project.CreatedAt,
            StartDate = project.StartDate
                ?? _dateTimeService.GetCurrentTime(),
            States = states.Select(s => new StateDto
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color,
                Description = s.Description,
                SortOrder = s.SortOrder,
            }),
        };

        return Result<ProjectDto>
            .Success(projectDto);
    }

    private async Task<Result<IEnumerable<StateEntity>>> CreateTemplateAsync(Guid projectId, Guid templateId, Guid userId)
    {
        var template = await _templateRepository
            .GetByIdAsync(templateId);

        if (template is null)
        {
            return Result<IEnumerable<StateEntity>>
                .Failure(TemplateErrors.NotFound);
        }

        var getTemplateStatesTask = _templateRepository
            .GetStatesAsync(templateId);

        var getTemplateTagsTask = _templateRepository
            .GetTagsAsync(templateId);

        await Task.WhenAll(getTemplateStatesTask, getTemplateTagsTask);

        var templateStates = getTemplateStatesTask.Result;
        var tempateTags = getTemplateTagsTask.Result;

        var states = templateStates
            .Select(s => new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = s.Name,
                Color = s.Color,
                Description = s.Description,
                SortOrder = s.SortOrder,
                ProjectId = projectId,
                CreatedAt = _dateTimeService.GetCurrentTime(),
                CreatedBy = userId,
            });

        await _stateRepository
            .CreateManyAsync(states);

        var tags = tempateTags
            .Select(t => new TagEntity
            {
                Id = Guid.NewGuid(),
                Name = t.Name,
                Color = t.Color,
                SortOrder = t.SortOrder,
                ProjectId = projectId,
                CreatedAt = _dateTimeService.GetCurrentTime(),
                CreatedBy = userId,
            });

        await _tagRepository
            .CreateManyAsync(tags);

        return Result<IEnumerable<StateEntity>>
            .Success(states);
    }
}
