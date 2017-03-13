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

    // Add extra methods here...
  }
}
