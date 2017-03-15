using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
      var applicationUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}";
      var headersDictionary = context.Request.Headers;
      var referrer = headersDictionary[HeaderNames.Referer].ToString();

      // if referer header is missing, or not using the current web site then consider this a problem
      if (string.IsNullOrEmpty(referrer) || !referrer.StartsWith(applicationUrl))
      {
        if (options.HandleWithHotLinkImage)
        {
          var unauthorizedImagePath = Path.Combine(rootFolder, options.HotLinkImagePath);
          await context.Response.SendFileAsync(unauthorizedImagePath);
        }
        // Fall through
        context.Response.StatusCode = options.StatusCode ?? StatusCodes.Status400BadRequest;
      }

      await next(context);
    }
  }
}
