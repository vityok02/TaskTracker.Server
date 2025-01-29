using Microsoft.Data.SqlClient;

namespace Persistence;

public interface ISqlConnectionFactory
{
    SqlConnection Create();
}
