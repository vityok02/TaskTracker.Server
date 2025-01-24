namespace Api;

public class DatabaseInitializerException : Exception
{
    public DatabaseInitializerException()
    { }

    public DatabaseInitializerException(string message)
        : base(message)
    { }

    public DatabaseInitializerException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
