using Microsoft.Data.SqlClient;

namespace Persistence.Abstractions;

public interface ISqlConnectionFactory
{
    SqlConnection Create();
}
