// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4;
using System.Collections.Generic;

namespace IdentityServer
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> Ids =>
      new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
      };
    public static IEnumerable<ApiResource> Apis =>
      new List<ApiResource>
      {            
          new ApiResource("api1", "My API")
      };        
    public static IEnumerable<Client> Clients =>
      new List<Client> 
      { 
        new Client
        {
          ClientId = "client",

          // no interactive user, use the clientid/secret for authentication
          AllowedGrantTypes = GrantTypes.ClientCredentials,

          // secret for authentication
          ClientSecrets =
          {
              new Secret("secret".Sha256())
          },

          // scopes that client has access to
          AllowedScopes = { "api1" }          
        },
        // new Client
        // {
        //   ClientId = "client2",

        //   // no interactive user, use the clientid/secret for authentication
        //   AllowedGrantTypes = GrantTypes.ClientCredentials,

        //   // secret for authentication
        //   ClientSecrets =
        //   {
        //       new Secret("secret".Sha256())
        //   },

        //   // scopes that client has access to
        //   AllowedScopes = { "api1" }          
        // },
        // JavaScript Client
        new Client
        {
          ClientId = "js",
          ClientName = "JavaScript Client",
          AllowedGrantTypes = GrantTypes.Code,
          
          RequirePkce = true,
          RequireClientSecret = false,

          RedirectUris =           { "http://localhost:5003/callback.html" },
          PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
          AllowedCorsOrigins =     { "http://localhost:5003" },

          AllowedScopes =
          {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            "api1"
          }
        }
      };
  }
}