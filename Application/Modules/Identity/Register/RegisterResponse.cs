using Application.Modules.Identity;

namespace Application.Modules.Users.Identity.RegisterUser;

public record RegisterResponse(
    Guid Id, TokenDto Token);
