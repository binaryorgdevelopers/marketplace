namespace Marketplace.Application.Common.Extensions;

public static class MappingExtensions
{
    public static IEnumerable<TSelect> MapTo<TEntity, TSelect>(this IEnumerable<TEntity> entity,
        Func<TEntity, TSelect> predicate)
        => entity.Select(predicate);
}