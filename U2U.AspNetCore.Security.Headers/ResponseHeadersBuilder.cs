using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace U2U.AspNetCore.Security.Headers
{
  public class ResponseHeadersBuilder
  {
    private Action<IHeaderDictionary> setter = null;

    public Action<IHeaderDictionary> Build() => setter;

    public void SetHeader(string header, string value)
    {
      setter += (headers) => headers.Add(header, value);
    }

    public void RemoveHeader(string header)
    {
      setter += (headers) => headers.Remove(header);
    }

    public void SetStrictTransportSecurity(StrictTransportSecurity hsts)
    {
      this.SetHeader("Strict-Transport-Security", hsts.ToHeader());
    }
    public void SetContentSecurityPolicy(ContentSecurityPolicy policy)
    {
      StringBuilder csp = new StringBuilder();

      if (policy.DefaultSrc != null)
      {
        csp.Append($"default-src {policy.DefaultSrc};");
      }
      if (policy.ScriptSrc != null)
      {
        csp.Append($"script-src {policy.ScriptSrc};");
      }
      if (policy.StyleSrc != null)
      {
        csp.Append($"style-src {policy.StyleSrc};");
      }

      this.SetHeader("Content-Security-Policy", csp.ToString());
    }
  }
}
