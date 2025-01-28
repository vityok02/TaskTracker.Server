namespace Domain;

public class ProjectMember
{
    public User User { get; private set; }
    public Project Project { get; private set; }
    public Role Role { get; private set; }

    public ProjectMember(User user, Project project)
    {
        User = user;
        Project = project;
    }

    private ProjectMember()
    { }

    public void SetRole(Role role)
    {
        Role = role;
    }
}
