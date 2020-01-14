using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.AddAuthentication("Bearer")
        .AddIdentityServerAuthentication(options => 
        {
          options.Authority = "http://localhost:5000";
          options.RequireHttpsMetadata = false;
          options.ApiName = "api1";
        });
        
      services.AddCors(options =>
      {
          // this defines a CORS policy called "default"
        options.AddPolicy("default", policy =>
        {
          policy.WithOrigins("http://localhost:5003")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    public void Configure(IApplicationBuilder app)
    {
      // if (env.IsDevelopment())
      // {
      //     app.UseDeveloperExceptionPage();
      // }

      app.UseRouting();

      app.UseCors("default");

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        // endpoints.MapGet("/", async context =>
        // {
        //     await context.Response.WriteAsync("Hello World!");
        // });
      });
    }
  }
}
