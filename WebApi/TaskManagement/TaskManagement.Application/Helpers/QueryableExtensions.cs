using System.Linq.Dynamic.Core;

namespace TaskManagement.Application.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, string? sortOrder = "asc")
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            if (!System.Text.RegularExpressions.Regex.IsMatch(sortBy, @"^[a-zA-Z0-9._]+$"))
                return query;

            var orderBy = $"{sortBy} {sortOrder}";
            return query.OrderBy(orderBy);
        }
    }
}