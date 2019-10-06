using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace vecihi.helper.Extensions
{
    public static class IQueryableExtensions
    {
        private static (ParameterExpression param, MemberExpression prop) QueryExpressions<Entity>(IQueryable<Entity> query, string property)
        {
            var param = Expression.Parameter(query.ElementType, "x");
            MemberExpression prop;

            if (property.Contains('.'))
            {
                string[] childProperties = property.Split('.');

                prop = Expression.Property(param, childProperties[0]);

                for (int i = 1; i < childProperties.Length; i++)
                    prop = Expression.Property(prop, childProperties[i]);
            }
            else
                prop = Expression.Property(param, property);

            return (param, prop);
        }
        public static IQueryable<Entity> Equal<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var constant = Expression.Convert(Expression.Constant(value), prop.Type);
            var body = Expression.Equal(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> Contains<Entity>(this IQueryable<Entity> query, string property, string value)
        {
            var methodInfo = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var constant = Expression.Constant(value);
            var body = Expression.Call(prop, methodInfo, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> GreaterThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var constant = Expression.Constant(value);
            var body = Expression.GreaterThan(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> GreaterThanOrEqual<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var constant = Expression.Constant(value);
            var body = Expression.GreaterThanOrEqual(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> LessThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var constant = Expression.Constant(value);
            var body = Expression.LessThan(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> LessThanOrEqual<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var constant = Expression.Constant(value);
            var body = Expression.LessThanOrEqual(prop, constant);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        private static (MethodCallExpression left, UnaryExpression right) DiffDays(MemberExpression prop, object value)
        {
            var methodInfo = typeof(SqlServerDbFunctionsExtensions).GetMethod("DateDiffSecond", new Type[] { typeof(DbFunctions), typeof(DateTime?), typeof(DateTime?) });

            var left = Expression.Call(
                methodInfo,
                Expression.Constant(EF.Functions, typeof(DbFunctions)),
                Expression.Convert(Expression.Constant(value), typeof(DateTime?)),
                Expression.Convert(prop, typeof(DateTime?)));

            var right = Expression.Convert(Expression.Constant(0), typeof(int?));

            return (left, right);
        }
        public static IQueryable<Entity> DiffDaysEqual<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);
            (MethodCallExpression left, UnaryExpression right) = DiffDays(prop, value);

            var body = Expression.Equal(left, right);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> DiffDaysGreaterThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);
            (MethodCallExpression left, UnaryExpression right) = DiffDays(prop, value);

            var body = Expression.GreaterThanOrEqual(left, right);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> DiffDaysLessThan<Entity>(this IQueryable<Entity> query, string property, object value)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);
            (MethodCallExpression left, UnaryExpression right) = DiffDays(prop, value);

            var body = Expression.LessThanOrEqual(left, right);

            return query.Where(Expression.Lambda<Func<Entity, bool>>(body, param));
        }
        public static IQueryable<Entity> OrderBy<Entity>(this IQueryable<Entity> query, string property)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            return query.OrderBy(Expression.Lambda<Func<Entity, object>>(Expression.Convert(prop, typeof(object)), param));
        }
        public static IQueryable<Entity> OrderByDescending<Entity>(this IQueryable<Entity> query, string property)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            return query.OrderByDescending(Expression.Lambda<Func<Entity, object>>(Expression.Convert(prop, typeof(object)), param));
        }
        public static async Task<double> SumAsync<Entity>(this IQueryable<Entity> query, string property)
        {
            (ParameterExpression param, MemberExpression prop) = QueryExpressions(query, property);

            var selector = Expression.Lambda<Func<Entity, double?>>(Expression.Convert(prop, typeof(double?)), param);

            return await query.SumAsync(selector) ?? 0;
        }
    }
}