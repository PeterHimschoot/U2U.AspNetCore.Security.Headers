using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace U2U.AspNetCore.Security.Headers
{
  public class ResponseHeadersMiddleware
  {
    private readonly RequestDelegate next;
    private readonly Action<IHeaderDictionary> headers;
    private readonly ResponseHeadersBuilder headerBuilder;

    public ResponseHeadersMiddleware(RequestDelegate next, ResponseHeadersBuilder reponseHeaders)
    {
      this.next = next;
      this.headerBuilder = reponseHeaders;
      // this.headers = reponseHeaders.Build();
    }

    public async Task Invoke(HttpContext context)
    {
      context.Response.OnStarting(state =>
      {
        var headers = this.headerBuilder.Build();
        headers(context.Response.Headers, context);
        return Task.FromResult(0);
      }, null);
      await next(context);
    }
  }
}
