﻿using IdentityModel;
using IdentityServer4.Models;

namespace IdentityService;

public static class Configuration
{
    public static IEnumerable<ApiResource> GetApis() =>
        new List<ApiResource>
        {
            new("Inventory")
        };

    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            new()
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "Inventory" }
            }
        };
}