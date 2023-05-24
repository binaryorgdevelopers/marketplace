namespace Shared.Extensions;

public static class StringExtensions
{
    public static string[] ToStringList(this string arg) => arg.Split("");
}