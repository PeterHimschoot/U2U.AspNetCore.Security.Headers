using System;
using System.Text;

public class StrictTransportSecurity
{
  public TimeSpan? MaxAge { get; set; }
  public bool IncludeSubdomains { get; set; }
  public bool Preload { get; set; }

  public string ToHeader() {
    StringBuilder hsts = new StringBuilder();
    if( this.MaxAge != null )
    {
      var seconds = this.MaxAge.Value.TotalSeconds;
      hsts.Append($"max-age={seconds};");
    }
    if(this.IncludeSubdomains == true) {
      hsts.Append($"includeSubdomains;");
    }
    if(this.Preload == true) {
      hsts.Append($"preload;");
    }
    return hsts.ToString();
  }
}
