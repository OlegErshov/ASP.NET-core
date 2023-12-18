using Duende.IdentityServer.Models;

namespace WEB.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope("api.read"),
            new ApiScope("api.write"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:7001/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:7001/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "api.read", "api.write" }
            },
            new Client
                {
                ClientId = "blazorApp",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RedirectUris = {
                "https://localhost:7004/authentication/login-callback" },
                PostLogoutRedirectUris = {
                "https://localhost:7004/authentication/logout-callback" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "api.read","api.write" }
                }
            };
    }
}