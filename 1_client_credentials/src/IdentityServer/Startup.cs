// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4;

namespace IdentityServer
{
    public class Startup
    {
        // public IWebHostEnvironment Environment { get; }

        // public Startup(IWebHostEnvironment environment)
        // {
        //     Environment = environment;
        // }
      private void CheckSameSite(HttpContext httpContext, CookieOptions options)
      {
          if (options.SameSite == SameSiteMode.None)
          {
              var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
              if (DisallowsSameSiteNone(userAgent))
              {
                  options.SameSite = (SameSiteMode) (-1);
              }
          }
      }

      private bool DisallowsSameSiteNone(string userAgent)
      {
          // Cover all iOS based browsers here. This includes:
          // - Safari on iOS 12 for iPhone, iPod Touch, iPad
          // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
          // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
          // All of which are broken by SameSite=None, because they use the iOS networking
          // stack.
          if (userAgent.Contains("CPU iPhone OS 12") ||
          userAgent.Contains("iPad; CPU OS 12"))
          {
              return true;
          }

          // Cover Mac OS X based browsers that use the Mac OS networking stack. 
          // This includes:
          // - Safari on Mac OS X.
          // This does not include:
          // - Chrome on Mac OS X
          // Because they do not use the Mac OS networking stack.
          if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
              userAgent.Contains("Version/") && userAgent.Contains("Safari"))
          {
              return true;
          }

          // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
          // and none in this range require it.
          // Note: this covers some pre-Chromium Edge versions, 
          // but pre-Chromium Edge does not require SameSite=None.
          if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
          {
              return true;
          }

          return false;
      }
      public void ConfigureServices(IServiceCollection services)
      {
        // uncomment, if you want to add an MVC-based UI
        //services.AddControllersWithViews();
        services.AddAuthorization();
        services.AddIdentityServer()
          .AddInMemoryApiResources(Config.Apis)
          .AddInMemoryClients(Config.Clients)
          // https://stackoverflow.com/questions/41010015/getting-scope-validating-error-in-identity-server-4-using-javascript-client-in-a
          .AddInMemoryIdentityResources(Config.Ids)
          .AddTestUsers(TestUsers.Users)
          .AddDeveloperSigningCredential();
        // Cookie policy stuff
        services.Configure<CookiePolicyOptions>(options =>
        {
          options.MinimumSameSitePolicy = (SameSiteMode) (-1);
          options.OnAppendCookie = cookieContext =>
            CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
          options.OnDeleteCookie = cookieContext =>
            CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });
      }

      public void Configure(IApplicationBuilder app)
      {
        // if (Environment.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        // }

        // uncomment if you want to add MVC
        //app.UseStaticFiles();
        //app.UseRouting();

        app.UseIdentityServer();
        app.UseRouting();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();
        // uncomment, if you want to add MVC
        //app.UseAuthorization();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});
      }
    }
}
