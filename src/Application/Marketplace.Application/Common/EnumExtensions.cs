using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common;

public static class EnumExtensions
{
    public static string RoleToString(this Roles entity)
        => Enum.GetName(typeof(Roles), entity);
}