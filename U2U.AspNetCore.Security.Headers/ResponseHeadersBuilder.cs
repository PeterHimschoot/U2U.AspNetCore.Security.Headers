using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace U2U.AspNetCore.Security.Headers
{
  public class ResponseHeadersBuilder
  {
    private Action<IHeaderDictionary, HttpContext> setter = null;

    public Action<IHeaderDictionary, HttpContext> Build() => setter;

    public void SetHeader(string header, string value)
    {
      setter += (headers, _) => headers.Add(header, value);
    }

    public void SetHeader(string header, Func<HttpContext, string> buildHeader)
    {
      setter += (headers, context) => headers.Add(header, buildHeader(context));
    }

    public void RemoveHeader(string header)
    {
      setter += (headers, _) => headers.Remove(header);
    }

    public void SetStrictTransportSecurity(StrictTransportSecurity hsts)
    {
      this.SetHeader("Strict-Transport-Security", hsts.ToHeader());
    }

    public void SetPublicKeyPinning(PublicKeyPinning hpkp)
    {
      this.SetHeader("Public-Key-Pins", hpkp.ToHeader());
    }

    public void SetContentSecurityPolicy(ContentSecurityPolicy policy)
    {
      string headerName =
        (policy.ReportOnly) ? "Content-Security-Policy-Report-Only" : "Content-Security-Policy";

      if (policy.SupportNonces)
      {
        this.SetHeader(headerName, policy.ToHeader);
      }
      else
      {
        this.SetHeader(headerName, policy.ToHeader(null));
      }
    }
  }
}
