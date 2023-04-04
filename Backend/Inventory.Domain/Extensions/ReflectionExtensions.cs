namespace Inventory.Domain.Extensions;

/// <summary>
/// Provides    methods for reflection operations
/// </summary>
public static class ReflectionExtensions
{
    public static bool InheritsOrImplements(this Type child, Type parent)
    {
        var par = parent;
        return InheritsOrImplementsHalf(child, ref parent) || par.IsAssignableFrom(child);
    }

    public static bool InheritsOrImplementsHalf(this Type child, ref Type parent)
    {
        parent = ResolveGenericTypeReflection(parent);
        var currentChild = child.IsGenericType ? child.GetGenericTypeDefinition() : child;

        while (currentChild != typeof(object))
        {
            if (parent == currentChild || HasAnyInterfaces(parent, currentChild)) return true;

            currentChild = currentChild.BaseType != null
                           && currentChild.BaseType.IsGenericType
                ? currentChild.BaseType.GetGenericTypeDefinition()
                : currentChild.BaseType;
            if (currentChild == null)
                return false;
        }

        return false;
    }


    private static bool HasAnyInterfaces(Type parent, Type child)
    {
        return child.GetInterfaces().Any(childInterface =>
        {
            var current = childInterface.IsGenericType ? child.GetGenericTypeDefinition() : child;
            return current == parent;
        });
    }

    private static Type ResolveGenericTypeReflection(Type parent)
    {
        var shouldUseGenericType = !(parent.IsGenericType && parent.GetGenericTypeDefinition() != parent);
        if (parent.IsGenericType && shouldUseGenericType) parent = parent.GetGenericTypeDefinition();
        return parent;
    }
}