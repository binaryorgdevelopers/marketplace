using System.Reflection;
using Marketplace.Application.Common.Builder.Extensions;

namespace Marketplace.Application.Common.Builder;

public static class QueryBuilder
{
    public static IEnumerable<PropertyInfo> GetSearchableProperties(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.GetProperties().Where(c =>
            c.PropertyType.IsSimpleType() && Attribute.IsDefined(c, typeof(SearchablePropertyAttribute)));
    }

    // public static Expression<Func<TSource,bool>>GetSearchProperties<TSource>(this.)
}