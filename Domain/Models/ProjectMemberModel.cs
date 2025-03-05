namespace Domain.Models;

public class ProjectMemberModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
