namespace Marketplace.Application.Common.Extensions;

public static class StringExtensions
{
    public static string[] StringToArray(this string str) => str.Split(",");
}