using System;
using System.Text;

public class StrictTransportSecurity
{
  public TimeSpan? MaxAge { get; set; }
  public bool IncludeSubdomains { get; set; } = true;
  public bool Preload { get; set; } = false;

  public string ToHeader()
  {
    if (this.Preload == true && this.IncludeSubdomains == false)
    {
      throw new ArgumentException("If Preload is true, then IncludeSubdomains should also be true.");
    }
    StringBuilder hsts = new StringBuilder();
    if (this.MaxAge != null)
    {
      var seconds = this.MaxAge.Value.TotalSeconds;
      hsts.Append($"max-age={seconds}");
    }
    if (this.IncludeSubdomains == true)
    {
      // preload requires includeSubdomains
      hsts.Append($"; includeSubdomains");
    }
    if (this.Preload == true)
    {
      hsts.Append($"; preload");
    }
    return hsts.ToString();
  }
}
