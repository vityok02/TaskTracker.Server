using Domain.Entities;
using Domain.Models.Common;

namespace Domain.Models;

public class ProjectMemberModel
{
    public UserInfoModel User { get; set; } = new();

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public RoleEntity Role { get; set; } = new();
}
