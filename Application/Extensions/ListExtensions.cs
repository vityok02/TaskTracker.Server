using Domain.Abstract;
using Domain.Entities;

namespace Application.Extensions;

public static class ListExtensions
{
    public static void InsertInOrderedList<T>(
        this List<T> tasks,
        Guid? beforeTaskId,
        T item)
        where T : BaseEntity
    {
        if (beforeTaskId.HasValue)
        {
            var beforeIndex = tasks
                .FindIndex(t => t.Id == beforeTaskId.Value);

            if (beforeIndex >= 0)
            {
                tasks.Insert(beforeIndex, item);
            }
            else
            {
                tasks.Add(item);
            }
        }
        else
        {
            tasks.Add(item);
        }
    }

    public static void Reorder<T>(
        this List<T> tasks)
        where T : SortableEntity
    {
        int i = 1;
        foreach (var t in tasks)
        {
            t.SortOrder = i++;
        }
    }
}
