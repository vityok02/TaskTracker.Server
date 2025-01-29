namespace Domain;

public class ProjectMember
{
    public User User { get; private set; }
    public Project Project { get; private set; }
    public Role Role { get; private set; }

    public ProjectMember(User user, Project project, Role role)
    {
        User = user;
        Project = project;
        Role = role;
    }

    private ProjectMember()
    { }

    public static ProjectMember Create(User user, Project project, Role role)
    {
        return new ProjectMember(user, project, role);
    }

    public void SetRole(Role role)
    {
        Role = role;
    }
}
