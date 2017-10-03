using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace U2U.AspNetCore.Security.Headers
{
  class PreventHotLinkingMiddleware
  {
    private readonly string rootFolder;
    private readonly RequestDelegate next;
    private PreventHotLinkingOptions options;

    public PreventHotLinkingMiddleware(RequestDelegate next, IHostingEnvironment env, PreventHotLinkingOptions options)
    {
      this.rootFolder = env.WebRootPath;
      this.options = options;
      this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      if (IsHotLinking(context))
      {
        context.Response.StatusCode = options.StatusCode ?? StatusCodes.Status401Unauthorized;
        if (options.HandleWithHotLinkImage)
        {
          var unauthorizedImagePath = Path.Combine(rootFolder, options.HotLinkImagePath);
          await context.Response.SendFileAsync(unauthorizedImagePath);
        }
      }
      else
      {
        await next(context);
      }
    }

    private bool IsHotLinking(HttpContext context)
    {
      var referer = GetRefererHeader(context);
      if (string.IsNullOrEmpty(referer))
      {
        return false;
      }
      var applicationUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}";
      if (referer.StartsWith(applicationUrl))
      {
        return false;
      }
      if (this.options.HasExceptions)
      {
        var refererUri = new Uri(referer);
        var refererHost = refererUri.Host;
        var refererPort = refererUri.Port;
        if (this.options.ExceptedHosts.Any(uri => uri.Host == refererHost && uri.Port == refererPort))
        {
          return false;
        }
      }
      return true;
    }

    private string GetRefererHeader(HttpContext context)
    {
      var headersDictionary = context.Request.Headers;
      var referrer = headersDictionary[HeaderNames.Referer].ToString();
      return referrer;
    }
  }
}
