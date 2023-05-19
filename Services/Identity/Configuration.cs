using IdentityModel;
using IdentityServer4.Models;

namespace Identity;

public static class Configuration
{
    public static IEnumerable<ApiScope> GetApis() =>
        new List<ApiScope>
        {
            new("inventory", "inventory"),
            new("basket", "basket"),
            new("ordering", "ordering")
        };

    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            new()
            {
                ClientId = "basket",
                ClientSecrets = { new Secret("basket".ToSha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "inventory", "basket", "ordering" }
            }
        };
    
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        yield return new IdentityResources.OpenId();
    }
}