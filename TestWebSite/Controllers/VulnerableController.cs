using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

public class VulnerableController : Controller
{
  public IActionResult OWASP() => View();
  public IActionResult ClickJacking() => View();
  public IActionResult HotLinking() => View();
  public IActionResult Inline() => View();
  public IActionResult UnsafeInline() => View();
  public IActionResult Sha256() => View();
  public IActionResult Nonces() => View();
  public IActionResult TargetBlank() => View();
  public IActionResult SameOrigin() => View();

  [Route("blogs")]
  public IActionResult Blogs() => View();

  private static Dictionary<int, string> redirects = new Dictionary<int, string> {
    {1, "http://blogs.u2u.be/peter/post/2017/07/02/enforce-https-everywhere-with-the-hsts-header.aspx"},
    {2, "http://blogs.u2u.be/peter/post/2017/06/19/protect-your-dotnet-core-website-with-content-security-policy.aspx"},
    {3, "http://blogs.u2u.be/peter/post/2017/07/04/pin-your-certificate-to-your-web-site-with-the-http-public-key-pinning-header.aspx"}
  };

  [Route("redirect/{id:int}")]
  public IActionResult RedirectToUrl(int id)
  {
    if (redirects.ContainsKey(id))
    {
      return Redirect(redirects[id]);
    }
    else
    {
      return RedirectToAction(nameof(VulnerableController.OWASP));
    }
  }
}
