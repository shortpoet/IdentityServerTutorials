using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace JavaScriptClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // public void ConfigureServices(IServiceCollection services)
        // {
        //   JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        //   services.AddAuthentication(options =>
        //   {
        //       options.DefaultScheme = "Cookies";
        //       options.DefaultChallengeScheme = "oidc";
        //   })
        //   .AddCookie("Cookies")
        //   .AddOpenIdConnect("oidc", options =>
        //   {
        //       options.Authority = "https://localhost:5003";
        //       options.RequireHttpsMetadata = false;

        //       options.ClientId = "js";
        //       // options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        //       options.ResponseType = "code";
        //       options.GetClaimsFromUserInfoEndpoint = true;

        //       // options.ClaimActions.MapJsonKey("location", "location");

        //       // asp.net core automatically stores resulting access and reresh token in the auth se4ssion
        //       options.SaveTokens = true;

        //       options.Scope.Add("api1");
        //       // adding additional scope
        //       options.Scope.Add("api2.full_access");
        //       options.Scope.Add("location");
        //       // options.Scope.Add("custom.profile.test");

        //       // enables support for refresh tokens
        //       options.Scope.Add("offline_access");

        //   });
        //   services.AddAuthorization();

        //   // services.AddCors(options => {
        //   //   // this defines a CORS policy called "default"
        //   //   options.AddPolicy("default", policy =>
        //   //   {
        //   //     policy.WithOrigins("https://localhost:5004;https://localhost:5001;https://localhost:5003")
        //   //     .AllowAnyHeader()
        //   //     .AllowAnyMethod();
        //   //   });
        //   // });
        // }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        public void Configure(IApplicationBuilder app)
        {
          app.UseDefaultFiles();
          app.UseStaticFiles();
          // app.UseCors("default");
          app.UseHttpsRedirection();
        }
    }
}
