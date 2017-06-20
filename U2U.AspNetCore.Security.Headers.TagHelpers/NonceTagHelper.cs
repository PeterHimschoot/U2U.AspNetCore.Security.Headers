using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace U2U.AspNetCore.Security.TagHelpers
{
  [HtmlTargetElement(Attributes = nameof(Nonce))]

  public class NonceTagHelper : TagHelper
  {
    public string Nonce { get; set; }

    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public string GenerateNonce()
    {
      byte[] randomBytes = new byte[32];
      var crnd = RandomNumberGenerator.Create();
      crnd.GetBytes(randomBytes);
      return System.Convert.ToBase64String(randomBytes);
    }

    private void AddNonceForTag(IDictionary<object, object> items, string tag, string nonce)
    {
      if (!items.ContainsKey(tag))
      {
        items.Add(tag, new List<string>());
      }
      var nonces = items[tag] as List<string>;
      nonces.Add(nonce);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      var items = this.ViewContext.HttpContext.Items;
      var nonce = GenerateNonce();
      output.Attributes.Add("nonce", nonce);
      AddNonceForTag(items, output.TagName, nonce);
    }
  }
}
