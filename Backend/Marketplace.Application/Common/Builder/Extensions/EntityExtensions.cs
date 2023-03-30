using Marketplace.Application.Common.Builder.Models;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Extensions;

namespace Marketplace.Application.Common.Builder.Extensions;

/// <summary>
/// Provides extensions specifically for entities
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Checks whether given type is entity
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns>True if given type is entity, otherwise false</returns>
    public static bool IsEntity(this Type type) => type.InheritsOrImplements(typeof(IIdentifiable));

    /// <summary>
    /// Gets direct child entities from a type
    /// </summary>
    /// <param name="type">Type to get direct child entities</param>
    /// <exception cref="ArgumentException"></exception>
    /// <returns>Set of direct child entities</returns>
    public static IEnumerable<Type> GetDirectChildEntities(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsEntity()) throw new ArgumentException();
        var result = type.GetProperties().Where(x => x.PropertyType.IsClass && x.PropertyType.IsEntity())
            .Select(x => x.PropertyType).ToList();

        return result.Distinct();
    }
}