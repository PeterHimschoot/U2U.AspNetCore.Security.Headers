using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace U2U.AspNetCore.Security.Headers
{
  public class ResponseHeadersMiddleware
  {
    private readonly RequestDelegate next;
    private readonly Action<IHeaderDictionary> headers;

    public ResponseHeadersMiddleware(RequestDelegate next, ResponseHeadersBuilder reponseHeaders)
    {
      this.next = next;
      this.headers = reponseHeaders.Build();
    }

    public async Task Invoke(HttpContext context)
    {
      await next(context);

      var responseHeaders = context.Response.Headers;
      this.headers(responseHeaders);
    }
  }
}
