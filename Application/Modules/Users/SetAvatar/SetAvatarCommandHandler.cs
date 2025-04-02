using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Users.SetAvatar;

internal sealed class SetAvatarCommandHandler
    : ICommandHandler<SetAvatarCommand, string>
{
    private readonly IBlobService _blobService;
    private readonly IUserRepository _userRepository;

    public SetAvatarCommandHandler(
        IBlobService blobService,
        IUserRepository userRepository)
    {
        _blobService = blobService;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(
        SetAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var extension = Path
            .GetExtension(command.FileName);

        string fileName = $"{command.UserId}{extension}";

        var url = await _blobService
            .UploadAsync(fileName, command.Content);

        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result<string>
                .Failure(UserErrors.NotFound);
        }

        user.AvatarUrl = url;

        await _userRepository
            .UpdateAsync(user);

        return Result<string>
            .Success(url);  
    }
}