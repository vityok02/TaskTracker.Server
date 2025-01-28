namespace Application.Abstract.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
}
