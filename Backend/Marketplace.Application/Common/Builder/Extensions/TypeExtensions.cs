namespace Marketplace.Application.Common.Builder;

public static class TypeExtensions
{
    public static bool IsSimpleType(this Type type) =>
        type.IsPrimitive || type.Equals(typeof(string)) || type.Equals(typeof(DateTime));
}