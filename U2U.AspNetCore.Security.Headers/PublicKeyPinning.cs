using System;
using System.Text;
using System.Collections.Generic;

public class PublicKeyPinning
{
  public List<string> Pins { get; set; }
  public TimeSpan? MaxAge { get; set; }
  public string ReportUri { get; set; }
  public bool IncludeSubdomains { get; set; } = true;
  public string ToHeader()
  {
    StringBuilder bob = new StringBuilder();

    foreach (var pin in Pins)
    {
      bob.Append($"pin-sha256={pin}");
    }

    if (MaxAge != null)
    {
      bob.Append($"max-age={this.MaxAge.Value.TotalSeconds}");
    }

    if (ReportUri != null)
    {
      bob.Append("report-uri={this.ReportUri}");
    }

    if (IncludeSubdomains)
    {
      bob.Append("includeSubdomains");
    }

    return bob.ToString();
  }
}
