using Marketplace.Domain.Constants;

namespace Marketplace.Application.Common.Extensions;

public static class EnumExtensions
{
    public static string RoleToString(this Roles entity)
        => Enum.GetName(typeof(Roles), entity);
}