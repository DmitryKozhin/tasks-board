using System.Collections.Generic;
using System.Linq;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Infrastructure.Extensions
{
    public static class DomainModelExtensions
    {
        public static int GetNextOrderNum<T>(this ICollection<T> orderableEntities) 
            where T: IOrderableEntity
        {
            if (orderableEntities.Any())
                return orderableEntities.Max(t => t.OrderNum) + 1;

            return default;
        }

        public static bool TryGetValue<T>(this T? nullable, out T value) where T : struct
        {
            value = default;

            if (!nullable.HasValue)
                return false;

            value = nullable.Value;
            return true;
        }
    }
}