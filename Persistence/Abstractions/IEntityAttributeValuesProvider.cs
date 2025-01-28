using System.Reflection;

namespace Persistence.Abstractions;

public interface IEntityAttributeValuesProvider<TEntity>
{
    string GetColumns(bool excludeKey = false);
    string GetColumnsAsProperties(bool excludeKey = false);
    string? GetKeyColumnName();
    IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false);
    string? GetKeyPropertyName();
    string GetPropertyNames(bool excludeKey = false);
    string GetTableName();
}
