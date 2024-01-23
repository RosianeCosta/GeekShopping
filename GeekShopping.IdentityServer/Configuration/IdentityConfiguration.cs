using Duende.IdentityServer;
using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResource =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiScope> ApiScope =>
          new List<ApiScope>
          {
                new ApiScope("geek_shopping","Geek Shopping Server"),
                new ApiScope(name: "read", "Read Data"),
                new ApiScope(name: "write", "Write Data"),
                new ApiScope(name: "delete", "Delete Data")
          };

        public static IEnumerable<Client> Clients =>
         new List<Client>
         {
             new Client
             {
                 ClientId = "clientId",
                    ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "read", "write", "profile"}
                },

             new Client
                {
                    ClientId = "geek_shopping",
                    ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:4430/signin-oidc"}, //applicationUrl frontend 
                    PostLogoutRedirectUris ={"https://localhost:4430/signout-callback-oidc"},
                    AllowedScopes = new List<string> 
                    {
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email,
                         "geek_shopping"
                    }
                }
         };
    }
}