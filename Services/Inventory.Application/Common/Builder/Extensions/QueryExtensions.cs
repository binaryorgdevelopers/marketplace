using System.Linq.Expressions;
using Inventory.Domain.Abstractions;
using Marketplace.Application.Common.Builder.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Application.Common.Builder.Extensions;

public static class QueryExtensions
{
    #region Query

    public static IQueryable<TEntity> ApplyQuery<TEntity>(this IQueryable<TEntity> source,
        IEntityQueryOptions<TEntity> queryOptions) where TEntity : class, IIdentifiable
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(queryOptions);

        var result = source;

        if (queryOptions.SearchOptions != null) result = result.ApplySearching(queryOptions.SearchOptions);
        return result;
    }

    #endregion


    #region Searching

    /// <summary>
    ///     Creates expression from filter options
    /// </summary>
    /// <param name="searchOptions">Filters</param>
    /// <typeparam name="TSource">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    private static Expression<Func<TSource, bool>> GetSearchExpression<TSource>(
        this SearchOptions<TSource> searchOptions) where TSource : class
    {
        ArgumentNullException.ThrowIfNull(searchOptions);

        //Get the properties of entity;
        var parameter = Expression.Parameter(typeof(TSource));

        var searchableProperties = typeof(TSource).GetSearchableProperties();

        //Add searchable properties
        var predicates = searchableProperties.Select(x =>
        {
            var member = Expression.PropertyOrField(parameter, x.Name);

            //Create expression based on type
            var compareMethod = x.PropertyType.GetCompareMethod(true);
            var argument = Expression.Constant(searchOptions.Keyword, x.PropertyType);
            var methodCaller = Expression.Call(member, compareMethod!, argument);
            return Expression.Lambda<Func<TSource, bool>>(methodCaller, parameter);
        }).ToList();

        //Join predicates
        var finalExpression = PredicateBuilder<TSource>.False;
        predicates.ForEach(x => finalExpression = PredicateBuilder<TSource>.Or(finalExpression, x));
        return finalExpression;
    }

    /// <summary>
    ///     Applies given searching options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="searchOptions">Search options</param>
    /// <typeparam name="TSource">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TSource> ApplySearching<TSource>(this IQueryable<TSource> source,
        SearchOptions<TSource> searchOptions) where TSource : class, IIdentifiable
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(searchOptions);

        //Include direct child entities if they have searchable properties too
        var searchExpressions = searchOptions.GetSearchExpression();

        if (searchOptions.IncludeChildren && typeof(TSource).IsEntity())
        {
            var relatedEntitiesProperty = typeof(TSource).GetDirectChildEntities().Select(x => new
            {
                Entity = x,
                SearchableProperties = x.GetSearchableProperties()
            });
            var matchingRelatedEntities = relatedEntitiesProperty?.Where(x => x.SearchableProperties.Any()).ToList();

            var predicates = matchingRelatedEntities?.Select(x =>
            {
                source.Include(x.Entity.Name);

                //Add matching entity predicates

                var parameter = Expression.Parameter(typeof(TSource));

                return x.SearchableProperties?.Select(y =>
                {
                    //Create predicate expression

                    var entity = Expression.PropertyOrField(parameter, x.Entity.Name);

                    var entityProperty = Expression.PropertyOrField(entity, y.Name);

                    //Create specific expression based on type
                    var compareMethod = y.PropertyType.GetCompareMethod(true);
                    var argument = Expression.Constant(searchOptions.Keyword, y.PropertyType);
                    var methodCaller = Expression.Call(entityProperty, compareMethod, argument);
                    return Expression.Lambda<Func<TSource, bool>>(methodCaller, parameter);
                });
            }).ToList();
            //Join predicate expressions

            predicates.ForEach(x =>
                x.ToList().ForEach(x => searchExpressions = PredicateBuilder<TSource>.Or(searchExpressions, x)));
        }

        return source.Where(searchExpressions);
    }

    /// <summary>
    ///     Applies given searching options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="searchOptions">Search options</param>
    /// <typeparam name="TSource">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IEnumerable<TSource> ApplySearching<TSource>(this IEnumerable<TSource> source,
        SearchOptions<TSource> searchOptions) where TSource : class
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(searchOptions);

        return source.Where(searchOptions.GetSearchExpression().Compile());
    }

    #endregion
}