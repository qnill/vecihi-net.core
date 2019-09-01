using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace vecihi.infrastructure
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Entity> Equal<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.Equal(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> Contains<Entity>(this IQueryable<Entity> query, string property, string value)
        {
            var methodInfo = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.Call(prop, methodInfo, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> GreaterThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.GreaterThan(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> GreaterThanOrEqual<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.GreaterThanOrEqual(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> LessThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.LessThan(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> LessThanOrEqual<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);
            var constant = Expression.Constant(value);
            var body = Expression.LessThanOrEqual(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> DiffDaysEqual<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var methodInfo = typeof(DbFunctions).GetMethod("DiffDays", new Type[] { typeof(DateTime?), typeof(DateTime?) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);

            var left = Expression.Call(
                methodInfo,
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)), Expression.Convert(prop, typeof(DateTime?)));
            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            var body = Expression.Equal(left, right);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> DiffDaysGreaterThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var methodInfo = typeof(DbFunctions).GetMethod("DiffDays", new Type[] { typeof(DateTime?), typeof(DateTime?) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);

            var left = Expression.Call(
                methodInfo,
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)), Expression.Convert(prop, typeof(DateTime?)));
            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            var body = Expression.GreaterThanOrEqual(left, right);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> DiffDaysLessThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            var methodInfo = typeof(DbFunctions).GetMethod("DiffDays", new Type[] { typeof(DateTime?), typeof(DateTime?) });

            var param = Expression.Parameter(query.ElementType, "x");
            var prop = Expression.Property(param, property);

            var left = Expression.Call(
                methodInfo,
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)), Expression.Convert(prop, typeof(DateTime?)));
            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            var body = Expression.LessThanOrEqual(left, right);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> OrderBy<Entity>(this IQueryable<Entity> query, string property)
        {
            var param = Expression.Parameter(query.ElementType, "o");
            var prop = Expression.Property(param, property);

            return query.OrderBy(Expression.Lambda<Func<Entity, object>>(prop, param));
        }
        public static IQueryable<Entity> OrderByDescending<Entity>(this IQueryable<Entity> query, string property)
        {
            var param = Expression.Parameter(query.ElementType, "o");
            var prop = Expression.Property(param, property);

            return query.OrderByDescending(Expression.Lambda<Func<Entity, object>>(prop, param));
        }
    }
}
