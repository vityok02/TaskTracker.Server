namespace Application.Dtos;

public class UserInfoDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }
}
