using Domain.Abstract;

namespace Domain;

public class User : BaseEntity
{
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public ICollection<Project> Projects { get; private set; } = [];

    private User(Guid id, string userName, string email, string password)
    {
        Id = id;
        UserName = userName;
        Email = email;
        Password = password;
    }

    private User()
    { }

    public static User Create(Guid id, string userName, string email, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (!email.Contains('@'))
        {
            throw new ArgumentException("Email must contain '@'.");
        }

        return new User(id, userName, email, password);
    }

    public void AddProject(Project project)
    {
        if (Projects.Any(p => p.Equals(project)))
        {
            throw new ArgumentException("Project already exists in user.");
        }

        Projects.Add(project);
    }
}
