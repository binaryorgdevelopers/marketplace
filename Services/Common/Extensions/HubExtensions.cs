namespace Shared.Extensions;

public static class HubExtensions
{
    public static string ToUserGroup(this Guid userId) => $"users:{userId}";
}