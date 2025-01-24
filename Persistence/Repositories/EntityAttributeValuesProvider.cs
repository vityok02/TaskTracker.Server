using Persistence.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Persistence.Repositories;

public class EntityAttributeValuesProvider<TEntity> : IEntityAttributeValuesProvider<TEntity>
    where TEntity : class
{
    public string GetColumns(bool excludeKey = false)
    {
        var type = typeof(TEntity);
        var columns = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttribute is not null ? columnAttribute.Name : p.Name;
            }));

        return columns;
    }

    public string GetColumnsAsProperties(bool excludeKey = false)
    {
        var type = typeof(TEntity);

        var columnsAsProperties = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Select(p =>
            {
                var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttribute is not null ? $"@{columnAttribute.Name} AS {p.Name}" : p.Name;
            }));

        return columnsAsProperties;
    }

    public string? GetKeyColumnName()
    {
        PropertyInfo[] properties = typeof(TEntity).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

            if (keyAttributes is not null && keyAttributes.Length > 0)
            {
                object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                if (columnAttributes is not null && columnAttributes.Length > 0)
                {
                    ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                    return columnAttribute?.Name ?? "";
                }
                else
                {
                    return property.Name;
                }
            }
        }

        return null;
    }

    public IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() is null);

        return properties;
    }

    public string? GetKeyPropertyName()
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() is not null).ToList();

        if (properties.Any())
        {
            return properties.FirstOrDefault()?.Name ?? "";
        }

        return null;
    }

    public string GetPropertyNames(bool excludeKey = false)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() is null);

        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        return values;
    }

    public string GetTableName()
    {
        var type = typeof(TEntity);
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();

        if (tableAttribute is not null)
        {
            return tableAttribute.Name;
        }

        return type.Name;
    }
}