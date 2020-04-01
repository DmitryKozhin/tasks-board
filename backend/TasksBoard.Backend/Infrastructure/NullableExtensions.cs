using System;

namespace TasksBoard.Backend.Infrastructure
{
    public static class NullableExtensions
    {
        public static bool TryGetValue<T>(this T? nullable, out T value) where T: struct
        {
            value = default;

            if (!nullable.HasValue) 
                return false;
            
            value = nullable.Value;
            return true;
        }
    }
}