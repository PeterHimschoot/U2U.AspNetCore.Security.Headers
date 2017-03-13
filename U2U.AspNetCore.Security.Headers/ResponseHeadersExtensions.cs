using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using U2U.AspNetCore.Security.Headers;

namespace Microsoft.AspNetCore.Builder
{
  public static class ResponseHeadersExtensions
  {
    public static IServiceCollection AddResponseHeaders(this IServiceCollection services)
    {
      return services;
    }

    public static IApplicationBuilder UseResponseHeaders(this IApplicationBuilder app, Action<ResponseHeadersBuilder> responseHeaders)
    {
      ResponseHeadersBuilder builder = new ResponseHeadersBuilder();
      responseHeaders(builder);

      return app.UseMiddleware<ResponseHeadersMiddleware>(builder);
    }
  }
}
