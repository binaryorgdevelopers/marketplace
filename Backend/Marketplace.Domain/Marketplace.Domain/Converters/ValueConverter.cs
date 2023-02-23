using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Marketplace.Domain.Converters;

public class ArrayToTConverter<T> : ValueConverter<T, string>
{
    public ArrayToTConverter(Expression<Func<T, string>> convertToProviderExpression,
        Expression<Func<string, T>> convertFromProviderExpression, ConverterMappingHints? mappingHints = null) : base(
        convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public ArrayToTConverter(Expression<Func<T, string>> convertToProviderExpression,
        Expression<Func<string, T>> convertFromProviderExpression, bool convertsNulls,
        ConverterMappingHints? mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression,
        convertsNulls, mappingHints)
    {
    }
}
public static class StringExtensions
{
    public static string[] StringToArray(this string str) => str.Split(",");
}