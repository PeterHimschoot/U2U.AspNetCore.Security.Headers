using System;
using System.Text;
using System.Collections.Generic;

public class PublicKeyPinning
{
  public List<string> Pins { get; set; }
  public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(1);
  public string ReportUri { get; set; }
  public bool IncludeSubdomains { get; set; } = true;
  public string ToHeader()
  {
    StringBuilder bob = new StringBuilder();

    foreach (var pin in Pins)
    {
      bob.Append($"pin-sha256={pin}; ");
    }

    bob.Append($"max-age={this.MaxAge.TotalSeconds}; ");

    if (IncludeSubdomains)
    {
      bob.Append("includeSubdomains; ");
    }

    if (ReportUri != null)
    {
      bob.Append("report-uri={this.ReportUri}");
    }

    return bob.ToString();
  }
}
