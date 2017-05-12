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
      context.Response.OnStarting(state =>
      {
        this.headers(context.Response.Headers);
        return Task.FromResult(0);
      }, null);
      await next(context);
    }
  }
}
