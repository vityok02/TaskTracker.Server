using Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("User")]
public class UserEntity : BaseEntity
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; } = string.Empty;
}
