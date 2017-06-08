using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

static class ContentSourceExtensions
{
  public static void AppendContentSources(this StringBuilder sb, List<string> sources)
  {
    foreach (var src in sources)
    {
      sb.Append($" {src}");
    }
    sb.Append("; ");
  }
}

public class ContentSecurityPolicy
{
  public static class Source
  {
    public static string Any = "*";
    public static string None = "'none'";
    public static string Self = "'self'";
  }

  public List<string> DefaultSrc { get; set; }
  public List<string> ScriptSrc { get; set; }
  public List<string> StyleSrc { get; set; }

  public string FrameAncestors { get; set; }

  public string ToHeader()
  {
    StringBuilder csp = new StringBuilder();

    if (this.DefaultSrc != null && this.DefaultSrc.Any())
    {
      csp.Append($"default-src");
      csp.AppendContentSources(this.DefaultSrc);
    }

    if (this.ScriptSrc != null && this.ScriptSrc.Any())
    {
      csp.Append($"script-src");
      csp.AppendContentSources(this.ScriptSrc);
    }

    if (this.StyleSrc != null && this.StyleSrc.Any())
    {
      csp.Append($"style-src");
      csp.AppendContentSources(this.StyleSrc);
    }

    if (this.FrameAncestors != null)
    {
      csp.Append($"frame-ancestors {this.FrameAncestors}");
    }

    return csp.ToString();
  }
}
