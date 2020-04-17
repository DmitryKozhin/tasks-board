using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.Internal;

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

        public static void UpdateOrder<T>(this ICollection<T> orderableEntities, T newEntity)
            where T : class, IOrderableEntity
        {
            var orderedEntities = orderableEntities.OrderBy(t => t.OrderNum).ToList();
            var hasElementAtPosition = orderedEntities.Any(t => t.OrderNum == newEntity.OrderNum);
            if (hasElementAtPosition)
                foreach (var entity in orderedEntities.Where(t => t.OrderNum >= newEntity.OrderNum))
                    entity.OrderNum += 1;
            else
                newEntity.OrderNum = orderedEntities.GetNextOrderNum();
        }

        public static void UpdateOrder<T>(this ICollection<T> orderableEntities, T newEntity, int newOrderNumber)
            where T : class, IOrderableEntity
        {
            var indexOfNewEntity = orderableEntities.OrderBy(t => t.OrderNum).IndexOf(newEntity);
            var orderedEntities = orderableEntities.Where(t => !t.Equals(newEntity)).OrderBy(t => t.OrderNum).ToList();
            if (newEntity.OrderNum < newOrderNumber)
                foreach (var entity in orderedEntities.Skip(indexOfNewEntity).Take(newOrderNumber - indexOfNewEntity))
                    entity.OrderNum -= 1;
            else
                foreach (var entity in orderedEntities.Skip(newOrderNumber).Take(indexOfNewEntity - newOrderNumber))
                    entity.OrderNum += 1;
        }

        public static void UpdateOrder<T>(this ICollection<T> orderableEntities)
            where T : class, IOrderableEntity
        {
            var orderedEntities = orderableEntities.OrderBy(t => t.OrderNum).ToList();
            for (var i = 0; i < orderedEntities.Count; i++)
                orderedEntities[i].OrderNum = i;
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