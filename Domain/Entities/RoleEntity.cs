using Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Role")]
public class RoleEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
