using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;

namespace ShoesOnContainers.Services.TokenServiceApi
{
    public class Config
    {
        //public static Dictionary<string, string> ClientUrls { get; private set; }
        public static Dictionary<string, string> GetUrls(IConfiguration configuration)
        {
            Dictionary<string, string> urls = new Dictionary<string, string>();

            urls.Add("Mvc", configuration.GetValue<string>("MvcClient"));

            return urls;

        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                 new ApiResource("basket", "Shopping Cart Api")
                 {
                     Scopes = { "basket" }
                 },
                 new ApiResource("orders", "Ordering Api")
                 {
                     Scopes = { "orders" }
                 },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("basket", "Shopping Cart Api"),
                new ApiScope("orders", "Ordering Api"),
            };
        }



        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
               // new IdentityResources.Email()
            };
        }
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrls)
        {

            return new List<Client>()
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = new [] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RedirectUris = {$"{clientUrls["Mvc"]}/signin-oidc"},
                    PostLogoutRedirectUris = {$"{clientUrls["Mvc"]}/signout-callback-oidc"},
                    AllowAccessTokensViaBrowser = false,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = new List<string>
                    {

                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                      //  IdentityServerConstants.StandardScopes.Email,
                         "orders",
                        "basket",

                    }

                }
            };
        }

    }
}