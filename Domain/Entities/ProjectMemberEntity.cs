using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("ProjectMember")]
public class ProjectMemberEntity
{
    public Guid UserId { get; set; }

    public Guid ProjectId { get; set; }

    public Guid RoleId { get; set; }
}
