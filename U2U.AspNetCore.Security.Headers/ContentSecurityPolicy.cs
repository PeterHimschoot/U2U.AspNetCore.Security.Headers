using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

static class ContentSourceExtensions
{
  private const string directiveSeperator = "; ";

  public static void AppendContentSources(this StringBuilder sb, List<string> sources)
  {
    foreach (var src in sources)
    {
      sb.Append($" {src}");
    }
    sb.Append(directiveSeperator);
  }
}

public class ContentSecurityPolicy
{
  public static class Source
  {
      /// <summary>
      /// Wildcard, allows any URL except data: blob: filesystem: schemes
      /// </summary>
    public static string Any = "*";
    /// <summary>
    /// Prevents loading resources from any source.
    /// </summary>
    public static string None = "'none'";
    /// <summary>
    /// Allows loading resources from the same origin (same scheme, host and port).
    /// </summary>
    public static string Self = "'self'";
    /// <summary>
    /// Allows loading resources via the data scheme (eg Base64 encoded images).
    /// </summary>
    public static string Data = "data:";
    /// <summary>
    /// Allows loading resources only over HTTPS on any domain
    /// </summary>
    public static string HttpsScheme = "https:";
    /// <summary>
    /// Allows use of inline source elements such as style attribute, onclick,
    /// or script tag bodies (depends on the context of the source it is applied to)
    /// and javascript: URIs
    /// </summary>
    public static string UnsafeInline = "'unsafe-inline'";
    /// <summary>
    /// Allows unsafe dynamic code evaluation such as JavaScript eval()
    /// </summary>
    public static string UnsafeEval = "'unsafe-eval'";
  }

  /// <summary>
  /// The default-src is the default policy for loading content such as JavaScript, Images,
  /// CSS, Font's, AJAX requests, Frames, HTML5 Media.!--
  /// </summary>
  public List<string> DefaultSrc { get; set; }
  /// <summary>
  /// Defines valid sources of JavaScript.
  /// </summary>
  public List<string> ScriptSrc { get; set; }
  /// <summary>
  /// Defines valid sources of stylesheets.
  /// </summary>
  public List<string> StyleSrc { get; set; }
  /// <summary>
  /// Defines valid sources of images.
  /// </summary>
  public List<string> ImageSrc { get; set; }
  /// <summary>
  /// Applies to XMLHttpRequest (AJAX), WebSocket or EventSource.
  /// If not allowed the browser emulates a 400 HTTP status code.
  /// </summary>
  public List<string> ConnectSrc { get; set; }
  /// <summary>
  /// Defines valid sources of fonts.
  /// </summary>
  public List<string> FontSrc { get; set; }
  /// <summary>
  /// Defines valid sources of plugins, eg <object>, <embed> or <applet>.
  /// </summary>
  public List<string> ObjectSrc { get; set; }

  /// <summary>
  /// Defines valid sources of audio and video, eg HTML5 <audio>, <video> elements.
  /// </summary>
  public List<string> MediaSrc { get; set; }

  /// <summary>
  /// Instructs the browser to POST reports of policy failures to this URI.
  /// You can also append -Report-Only to the HTTP header name to instruct the browser
  /// to only send reports (does not block anything)
  /// </summary>
  public string ReportUri { get; set; }

  /// <summary>
  /// Append -Report-Only to the HTTP header name.
  /// </summary>
  public bool ReportOnly { get; set; }

  /// <summary>
  /// Defines valid sources for web workers and nested browsing
  /// contexts loaded using elements such as <frame> and <iframe>
  /// </summary>
  public List<string> ChildSrc { get; set; }

  /// <summary>
  /// Defines valid sources that can be used as a HTML <form> action
  /// </summary>
  public List<string> FormAction { get; set; }

/// <summary>
/// Defines valid sources for embedding the resource
/// using <frame> <iframe> <object> <embed> <applet>.
/// Setting this directive to 'none' should be roughly
/// equivalent to X-Frame-Options: DENY
/// </summary>
  public List<string> FrameAncestors { get; set; }

/// <summary>
/// Defines valid MIME types for plugins invoked via <object> and <embed>.
/// To load an <applet> you must specify application/x-java-applet.
/// </summary>
  public List<string> PluginTypes { get; set; }

  public string ToHeader(HttpContext context)
  {
// TODO : retrieve nonces from context


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

    if (this.ImageSrc != null && this.ImageSrc.Any())
    {
      csp.Append($"img-src");
      csp.AppendContentSources(this.ImageSrc);
    }

    if (this.ConnectSrc != null && this.ConnectSrc.Any())
    {
      csp.Append($"connect-src");
      csp.AppendContentSources(this.ConnectSrc);
    }

    if (this.FontSrc != null && this.FontSrc.Any())
    {
      csp.Append($"connect-src");
      csp.AppendContentSources(this.FontSrc);
    }

    if (this.ObjectSrc != null && this.ObjectSrc.Any())
    {
      csp.Append($"object-src");
      csp.AppendContentSources(this.ObjectSrc);
    }

    if (this.MediaSrc != null && this.MediaSrc.Any())
    {
      csp.Append($"media-src");
      csp.AppendContentSources(this.MediaSrc);
    }

    if (this.ChildSrc != null && this.ChildSrc.Any())
    {
      csp.Append($"child-src");
      csp.AppendContentSources(this.ChildSrc);
    }

    if (this.FormAction != null && this.FormAction.Any())
    {
      csp.Append($"form-action");
      csp.AppendContentSources(this.FormAction);
    }

    if (this.FrameAncestors != null && this.FrameAncestors.Any())
    {
      csp.Append($"frame-ancestors");
      csp.AppendContentSources(this.FrameAncestors);
    }

    if( this.PluginTypes!= null && this.PluginTypes.Any()) {
      csp.Append($"plugin-types");
      csp.AppendContentSources(this.PluginTypes);
    }

    return csp.ToString();
  }
}
