using Microsoft.AspNetCore.Mvc;

public class VulnerableController : Controller {
  public IActionResult ClickJacking() => View();
  public IActionResult HotLinking() => View();
}
