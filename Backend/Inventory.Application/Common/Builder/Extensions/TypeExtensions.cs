using System.Linq.Expressions;
using System.Reflection;
using Marketplace.Application.Common.Builder.Models;

namespace Marketplace.Application.Common.Builder.Extensions;

public static class TypeExtensions
{
    public static bool IsSimpleType(this Type type) =>
        type.IsPrimitive || type == typeof(string) || type == typeof(DateTime);

    public static MethodInfo GetCompareMethod(this Type type, bool searchComparing = false)
    {
        if (!type.IsSimpleType()) throw new ArgumentException("Not a primitive type");
        var methodName = type == typeof(string) && searchComparing ? "Contains" : "Equals";
        return type.GetMethod(methodName, new[] { type }) ?? throw new InvalidOperationException("Method not found");
    }

    public static object GetValue(this QueryFilter filter, Type type)
    {
        ArgumentNullException.ThrowIfNull(filter);
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsSimpleType()) throw new ArgumentException("Not a primitive type");
        if (type == typeof(string)) return filter.Value;
        var parameter = Expression.Parameter(typeof(string));
        var parseMethod = type.GetMethod("Parse", new[] { typeof(string) }) ??
                          throw new InvalidOperationException("Method not found");
        var argument = Expression.Constant(filter.Value);
        var methodCaller = Expression.Call(parseMethod, argument);
        var returnConverter = Expression.Convert(methodCaller, typeof(object));
        var function = Expression.Lambda<Func<string, object>>(returnConverter, parameter).Compile();

        return function.Invoke(filter.Value);
    }
}