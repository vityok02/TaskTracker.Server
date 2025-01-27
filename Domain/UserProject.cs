namespace Domain;

public class UserProject
{
    public User User { get; private set; }
    public Project Project { get; private set; }
    public Role Role { get; private set; }

    public UserProject(User user, Project project)
    {
        User = user;
        Project = project;
    }

    public void SetRole(Role role)
    {
        Role = role;
    }
}
