namespace Domain.Models;

public class ProjectMemberModel
{
    public UserInfoModel User { get; set; } = new();

    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
