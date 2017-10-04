using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
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

      if (!env.IsDevelopment())
      {
        // Redirect to HTTPS
        var options = new RewriteOptions()
        .AddRedirectToHttps();
        app.UseRewriter(options);
      }

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UsePreventHotLinking( options => {
        options.HotLinkImagePath = "images/HotLink.jpeg";
        options.ExceptedHosts = new List<Uri>{ new Uri("https://blogs.u2u.be") };
      });

      app.UseResponseHeaders(builder =>
      {
        // builder.SetHeader("Header", "Value");

        builder.SetStrictTransportSecurity(new StrictTransportSecurity
        {
          MaxAge = TimeSpan.FromDays(1),
          IncludeSubdomains = true,
          Preload = false
        });

        builder.SetPublicKeyPinning(new PublicKeyPinning
        {
          MaxAge = TimeSpan.FromDays(10),
          IncludeSubdomains = true,
          Pins = new List<string> {
              "yh0kYiYm4YN+0DAKp4bB16pGqrQq9btXHMeR9jz834o=", // current certificate
              "YLh1dUR9y6Kja30RrAn7JKnbQG/uEtLMkBgFF2Fuihg=", // Let's Encrypt Authority X3
              "SEnt86CqqSYlSIlLcfnKdJdoS8NJG1EG+/5b5qtvmUY="  // backup cert
              }
        });

        builder.SetContentSecurityPolicy(new ContentSecurityPolicy()
        {
          SupportNonces = true,
          FrameAncestors = new List<string> { ContentSecurityPolicy.Source.None },
          DefaultSrc = new List<string> {
              ContentSecurityPolicy.Source.Self,
              "https://www.u2u.be"
          },
          ScriptSrc = new List<string> {
              ContentSecurityPolicy.Source.Self,
              // ContentSecurityPolicy.Source.UnsafeInline,
              "https://ajax.aspnetcdn.com",
              "'sha256-d5/o7Lq1BQizlE+7YpLcN8kzeapQhf2bAgOX+645XGI='"
          },
          StyleSrc = new List<string> {
              ContentSecurityPolicy.Source.Self,
              "https://ajax.aspnetcdn.com",
              "'sha256-pTnn8NGuYdfLn7/v3BQ2pYxjz73VjHU2Wkr6HjgUgVU='"
          }
        });
      });

      app.UseStaticFiles(new StaticFileOptions
      {

        ServeUnknownFileTypes = true
      });

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Vulnerable}/{action=OWASP}/{id?}");
      });
    }
  }
}
