namespace Domain.Models;

public class UserInfoModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }
}
