using System.Linq.Expressions;

namespace Order.Infrastructure.Common
{
    public static class CommonUtilities
    {
        public static IQueryable<T> ApplyStringFilter<T>(this IQueryable<T> query,Expression<Func<T, string?>> selector,string? value,bool use_contains)
        {
            if (string.IsNullOrWhiteSpace(value))
                return query;

            string needle = value.Trim();

            return use_contains ? query.Where(BuildContains(selector, needle)) : query.Where(BuildEquals(selector, needle));
        }

        private static Expression<Func<T, bool>> BuildContains<T>(
            Expression<Func<T, string?>> selector,
            string needle)
        {
            var param = selector.Parameters[0];
            var member = selector.Body;

            var not_null = Expression.NotEqual(member, Expression.Constant(null, typeof(string)));
            var contains = Expression.Call(member, nameof(string.Contains), Type.EmptyTypes, Expression.Constant(needle));

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(not_null, contains), param);
        }

        private static Expression<Func<T, bool>> BuildEquals<T>(
            Expression<Func<T, string?>> selector,
            string needle)
        {
            var param = selector.Parameters[0];
            var member = selector.Body;

            var not_null = Expression.NotEqual(member, Expression.Constant(null, typeof(string)));
            var equals = Expression.Equal(member, Expression.Constant(needle));

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(not_null, equals), param);
        }

        public static IQueryable<T> ApplyPaging<T>(
                this IQueryable<T> query,
                int page,
                int page_size,
                int default_page_size = 20,
                int max_page_size = 200)
        {
            int normalized_page = page <= 0 ? 1 : page;

            int normalized_page_size = page_size <= 0 ? default_page_size : page_size;
            if (normalized_page_size > max_page_size)
                normalized_page_size = max_page_size;

            int skip = (normalized_page - 1) * normalized_page_size;

            return query.Skip(skip).Take(normalized_page_size);
        }
    }
}
