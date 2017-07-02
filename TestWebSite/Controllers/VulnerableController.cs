using Microsoft.AspNetCore.Mvc;

public class VulnerableController : Controller {
  public IActionResult OWASP() => View();
  public IActionResult ClickJacking() => View();
  public IActionResult HotLinking() => View();
  public IActionResult Inline() => View();
  public IActionResult UnsafeInline() => View();
  public IActionResult Sha256() => View();
  public IActionResult Nonces() => View();
}
