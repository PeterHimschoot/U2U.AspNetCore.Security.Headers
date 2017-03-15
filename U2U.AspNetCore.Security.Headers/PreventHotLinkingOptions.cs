using System;
using System.Collections.Generic;
using System.Text;

namespace U2U.AspNetCore.Security.Headers
{
  public class PreventHotLinkingOptions
  {
    public string HotLinkImagePath { get; set; }

    public bool HandleWithHotLinkImage => !string.IsNullOrEmpty(HotLinkImagePath);

    public int? StatusCode { get; set; }

    public bool ReturnStatusCode => StatusCode.HasValue;
  }
}
