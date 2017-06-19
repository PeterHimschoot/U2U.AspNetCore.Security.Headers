using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestWebSite
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddMvc();
      services.AddNameOf();
      services.AddMarkdown();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UsePreventHotLinking("images/HotLink.jpeg");

      app.UseResponseHeaders(builder =>
      {
        builder.SetStrictTransportSecurity(new StrictTransportSecurity
        {
          MaxAge = TimeSpan.FromDays(1),
          IncludeSubdomains = true,
          Preload = false
        });

        builder.SetPublicKeyPinning(new PublicKeyPinning
        {
          MaxAge = TimeSpan.FromMinutes(10),
          IncludeSubdomains = true,
          Pins = new List<string> {
              "yh0kYiYm4YN+0DAKp4bB16pGqrQq9btXHMeR9jz834o=", // current certificate
              "SEnt86CqqSYlSIlLcfnKdJdoS8NJG1EG+/5b5qtvmUY=" // demo Certificate Signing Request
              }
        });

        builder.SetContentSecurityPolicy(new ContentSecurityPolicy()
        {
          FrameAncestors = new List<string>{ ContentSecurityPolicy.Source.None },
          DefaultSrc = new List<string> {
              ContentSecurityPolicy.Source.Self
          },
          ScriptSrc = new List<string> {
              ContentSecurityPolicy.Source.Self,
              "https://ajax.aspnetcdn.com",
              "'sha256-9FLqevjfni80Rt3z09prhe9sCFtWYVSaHXIcLZKWC+s='"
          },
          StyleSrc = new List<string> {
              ContentSecurityPolicy.Source.Self,
              "https://ajax.aspnetcdn.com"
          }
        });
      });

      app.UseStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
