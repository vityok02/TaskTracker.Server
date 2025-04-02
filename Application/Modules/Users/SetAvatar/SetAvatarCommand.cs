using Application.Abstract.Messaging;

namespace Application.Modules.Users.SetAvatar;

public sealed record SetAvatarCommand(
    string FileName,
    Stream Content,
    Guid UserId)
    : ICommand<string>;
