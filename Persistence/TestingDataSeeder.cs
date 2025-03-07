using Dapper;
using Microsoft.Data.SqlClient;
using Persistence.Abstractions;

namespace Persistence;

public static class TestingDataSeeder
{
    public static async Task SeedData(ISqlConnectionFactory connectionFactory)
    {
        using var connection = connectionFactory.Create();

        await SeedUsers(connection);
    }

    private static async Task SeedUsers(SqlConnection connection)
    {
        var query = @"
            INSERT INTO [User] ([Id], [Username], [Password], [Email])
            VALUES
            (NEWID(), 'Alice01', 'StrongPassword123', 'alice01@gmail.com'),
            (NEWID(), 'Bob_Rocks', 'StrongPassword123', 'bob.rocks@yahoo.com'),
            (NEWID(), 'CharlieX', 'StrongPassword123', 'charliex@mail.com'),
            (NEWID(), 'DianaW', 'StrongPassword123', 'diana.w@hotmail.com'),
            (NEWID(), 'EvanCoder', 'StrongPassword123', 'evan.coder@outlook.com'),
            (NEWID(), 'Fiona98', 'StrongPassword123', 'fiona98@gmail.com'),
            (NEWID(), 'George777', 'StrongPassword123', 'george777@aol.com'),
            (NEWID(), 'HannahM', 'StrongPassword123', 'hannah.m@icloud.com');";

        await connection.ExecuteAsync(query);
    }
}
