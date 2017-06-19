using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Cryptography;
using System.Text;

namespace U2U.AspNetCore.Security.TagHelpers
{
  [HtmlTargetElement(Attributes = nameof(Nonce))]

  public class NonceTagHelper : TagHelper
  {
    public bool Nonce { get; set; }

    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public string GenerateNonce()
    {
      byte[] randomBytes = new byte[32];
      var crnd = RandomNumberGenerator.Create();
      crnd.GetBytes(randomBytes);
      return System.Convert.ToBase64String(randomBytes);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      //   var items = this.ViewContext.HttpContext.Items;
      var nonce = GenerateNonce();
      output.Attributes.Add("nonce", nonce);

      if (output.TagName == "script")
      {
      }
      else if (output.TagName == "")
      {
      }
    }
  }
}
