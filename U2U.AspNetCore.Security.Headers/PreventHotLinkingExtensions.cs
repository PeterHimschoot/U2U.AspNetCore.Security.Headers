using System;
using System.Collections.Generic;
using System.Text;
using U2U.AspNetCore.Security.Headers;

namespace Microsoft.AspNetCore.Builder
{
  public static class PreventHotLinkingExtensions
  {
    public static IApplicationBuilder UsePreventHotLinking(this IApplicationBuilder builder, string hotLinkImagePath)
    {
      builder.UsePreventHotLinking(new PreventHotLinkingOptions
      {
        HotLinkImagePath = hotLinkImagePath
      });
      return builder;
    }

    public static IApplicationBuilder UsePreventHotLinking(this IApplicationBuilder builder, int statusCode)
    {
      builder.UsePreventHotLinking(new PreventHotLinkingOptions
      {
        StatusCode = statusCode
      });
      return builder;
    }

    public static IApplicationBuilder UsePreventHotLinking(this IApplicationBuilder builder, Action<PreventHotLinkingOptions> setOptions)
    {
      var options = new PreventHotLinkingOptions();
      setOptions(options);
      return builder.UsePreventHotLinking(options);
    }


    public static IApplicationBuilder UsePreventHotLinking(this IApplicationBuilder builder, PreventHotLinkingOptions options)
    {
      builder.UseMiddleware<PreventHotLinkingMiddleware>(options);
      return builder;
    }
  }
}
