using System;
using System.Text;
public class ContentSecurityPolicy {

   public static class Source {
    public static string Any = "*";
    public static string None = "'none'";
    public static string Self = "'self'";
  }

   public string DefaultSrc { get; set; }
   public string ScriptSrc { get; set; }
   public string StyleSrc { get; set; }

  public string ToHeader()
  {
    StringBuilder csp = new StringBuilder();

    if (this.DefaultSrc != null)
    {
      csp.Append($"default-src {this.DefaultSrc};");
    }
    if (this.ScriptSrc != null)
    {
      csp.Append($"script-src {this.ScriptSrc};");
    }
    if (this.StyleSrc != null)
    {
      csp.Append($"style-src {this.StyleSrc};");
    }
    return csp.ToString();
  }
}
