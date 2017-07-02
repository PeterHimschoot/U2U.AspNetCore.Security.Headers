# U2U.AspNetCore.Security.Headers package

Use this package for adding the following headers 

1. Content Security Policy (**CSP**)
2. HTTP Strict Transport Security (**HSTS**)
3. HTTP Public Key Pinning (**HPKP**)

This package also contains middleware to check the REFERER header.

---

# Protect your dotnet core website with Content Security Policy

The new `Content-Security-Policy` HTTP response header helps you reduce **XSS** risks on modern browsers by declaring what dynamic resources are allowed to load via a HTTP Header.

For example, with the CSP header you can block inline scripts from executing, effectively stopping simple XSS attacks.

---

## What is the Content Security Policy

HTTP headers are used by Servers and Browsers to talk to one another. For example the server can tell the browser what kind of content it is sending using the `Content-Type` and `Content-Length` header.
Every web page is built-up from different content sources, for example the html comes from the server, but your style might come from a CDN server such as bootstrap.
The new `Content-Security-Policy` is used by the server to tell the browser which content-sources it can use, for example:

```
Content-Security-Policy:default-src 'self'; style-src 'self' https://ajax.aspnetcdn.com
```

This header tells the browser to only use html from the server itself, and only to use styles from the server and the aspnetcdn server. Browsers that support CSP will not use any other content sources.

But wait! There is more. You can also tell the browser not to load your content into another page, protecting against [Clickjacking](https://en.wikipedia.org/wiki/Clickjacking). You can use the `frame-ancestors` directive for that:

```
Content-Security-Policy:default-src 'self'; script-src 'self' https://ajax.aspnetcdn.com ; style-src 'self' https://ajax.aspnetcdn.com; frame-ancestors 'none';
```

All the different kinds of content sources and directives can be found [here](https://content-security-policy.com/#source_list)

---

## Using inline scripts with CSP

Maybe you are using some external inline script, such as [Google Analytics](https://analytics.google.com/) or [Microsoft Application Insights](https://azure.microsoft.com/en-us/services/application-insights/). Normally CSP will block any inline script or style. So how can I use my inline script?

One option would be to use the `'unsafe-inline'` content source, but this allows any inline script to execute! Luckily there is another way.

### Using hashes to allow inline scripts and styles

When CSP has been enabled, and you have an inline script on your page, your browser will not execute it. You will find in your browser's Console window some remark about it. For example in Chrome:

```
Refused to execute inline script because it violates the following Content Security Policy directive
```

Chrome also suggest a way to get around this, and even displays a hash for the inline script:

```
Either the 'unsafe-inline' keyword, a hash ('sha256-e3wuJEA9ZnrbftKXWc68bpGC5pLCehsGKmy02Qh9h74='), or a nonce ('nonce-...') is required to enable inline execution.
```

So the solution is to include this hash value in your content sources:

```
default-src 'self'; script-src 'self' https://ajax.aspnetcdn.com 'sha256-gKHd+pSZOJ3MwBsFalomyNobAcinjJ44ArqbIKlcniQ='; style-src 'self' https://ajax.aspnetcdn.com 'sha256-pTnn8NGuYdfLn7/v3BQ2pYxjz73VjHU2Wkr6HjgUgVU='; frame-ancestors 'none';
```

As you can see in the example above, you can also use this for inline styles.

## Using nonces to allow inline scripts and styles

The mayor disadvantage of the hash is that you need to recalculate and update the hash value whenever you update the script/style. So if you update the script (or dynamically generate it) you will want to use a nonce.

A nonce is a 'number used only once'

You need to generate for each request a unique nonce for each inline script and inline style and include it in the CSP header:

```
Content-Security-Policy:default-src 'self'; script-src 'self' https://ajax.aspnetcdn.com 'sha256-gKHd+pSZOJ3MwBsFalomyNobAcinjJ44ArqbIKlcniQ=' 'nonce-1LCV8O37L47QVufyugd6rqoebY+OAQGq8iajMbdy3B8='; style-src 'self' https://ajax.aspnetcdn.com 'sha256-pTnn8NGuYdfLn7/v3BQ2pYxjz73VjHU2Wkr6HjgUgVU=' 'nonce-ZUqNLKpiwM9Hru6BjlIx6DtREfGXO2c38CCzMAW6TQ0='; frame-ancestors 'none';
```

You also need to attribute your scripts and styles with a nonce attribute, matching the nonce from the header.

```
<script nonce="1LCV8O37L47QVufyugd6rqoebY&#x2B;OAQGq8iajMbdy3B8=">
```

---

## Adding the CSP header in .NET Core

To make adding the CSP header easy in .NET Core I have built two NuGet packages: `U2U.AspNetCore.Security.Headers` and `U2U.AspNetCore.Security.Headers.TagHelpers`.

So start by adding them to your project:

```
<PackageReference Include="U2U.AspNetCore.Security.Headers" Version="1.1.0" />
<PackageReference Include="U2U.AspNetCore.Security.Headers.TagHelpers" Version="1.1.0" /> 
```
 
### U2U.AspNetCore.Security.Headers

This package allows you to add headers to your response, such as the CSP header. Call `UseResponseHeaders` in your `Startup.Configure` method:

```
app.UseResponseHeaders(builder =>
{
  ...
}
```

You can set any header like this:

```
builder.SetHeader("SomeHeader", "SomeValue")
```

You can set the CSP header using the `SetContentSecurityPolicy` method:

```
builder.SetContentSecurityPolicy(new ContentSecurityPolicy()
  {
    ...
  }
```

Now select the directives you need with their content sources:

```
DefaultSrc = new List<string> {
  ContentSecurityPolicy.Source.Self
},
ScriptSrc = new List<string> {
  ContentSecurityPolicy.Source.Self,
  "https://ajax.aspnetcdn.com",
  "'sha256-gKHd+pSZOJ3MwBsFalomyNobAcinjJ44ArqbIKlcniQ='"
},
StyleSrc = new List<string> {
  ContentSecurityPolicy.Source.Self,
  "https://ajax.aspnetcdn.com",
  "'sha256-pTnn8NGuYdfLn7/v3BQ2pYxjz73VjHU2Wkr6HjgUgVU='"
}
```

And what about nonces?

### U2U.AspNetCore.Security.Headers.TagHelpers

Using nonces means that you need to generate a cryptographically randon nonce, and attach it to the header and the script of style tag. This package makes that easy through a nonce taghelper.

If you're not familiar with taghelpers, click this [link](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro)

First of all enable nonces in `Startup.Configure`:

```
builder.SetContentSecurityPolicy(new ContentSecurityPolicy()
{
  SupportNonces = true,
```

Next add the nonce taghelper to your views. The easiest way is to add following to `_ViewImports.cshtml`:

```
@addTagHelper *, U2U.AspNetCore.Security.Headers.TagHelpers
```

Now look for your inline script and style tag(s) and add the nonce attribute:

```
<script nonce="true">alert('Use the Nonce!');</script>
```

Start your website and use the browser debugger to look at the CSP header:

```
Content-Security-Policy:default-src 'self'; 
script-src 'self' https://ajax.aspnetcdn.com 
'nonce-Gl9JnGKKw9+0+fThsPtVdYtraPLwxWDtB4Qq7qMKH0w=';
style-src 'self' https://ajax.aspnetcdn.com  'nonce-KX0fql/urMHxnZGnDqNoyOljycR/e8nNv2bsjk//sS8='; 
```

Your script tags should also include the nonce value:

```
<script nonce="Gl9JnGKKw9&#x2B;0&#x2B;fThsPtVdYtraPLwxWDtB4Qq7qMKH0w=">
  alert('Use the Nonce!');
</script>
<script nonce="stjl3RNNOKDytWwDlWb8Rr2FGmNAmEdykWaCCPc10TQ=">
  alert('Also ok!');
</script>
```

## Does your browser support `CSP`?

The easiest way to find out is to visit [CSP-Browser-Test](https://content-security-policy.com/browser-test/)

But generally, if you are using the latest version of a modern browser, it should support CSP.

---

# Enforce HTTPS everywhere with the HSTS header

With the **HTTP Strict Transport Security** header you can tell the browser to only use secure HTTP (so **HTTPS**) for downloading your website's content. 

This blog post explains the header, but also how to add the HSTS header to your .NET Core website using the `U2U.AspNetCore.Security.Headers` package.

## So how does it work?

Modern browsers can be instructed about how to handle your content in different ways. You can tell it for example to only download scripts from your web site with the Content Security Policy header. You can also tell it to use HTTPS to download your content.

The HSTS header is a great way to protect against [Downgrade attacks](https://en.wikipedia.org/wiki/Downgrade_attack) and [SSL strip attack](https://avicoder.me/2016/02/22/SSLstrip-for-newbies/)

The header itself is pretty straighforward. Here's an example:

```
Strict-Transport-Security:max-age=86400; includeSubdomains; preload;
```

## Options in the HSTS header - the max-age directive

The **max-age** directive marks the period in which insecure requests cannot be made...
The units are in seconds, and the duration is reset on every response of the response header.

**You do have the realize that all requests will now use https, including .css, .js, etc...**

If you want to get rid of the header (especially during development) you can do so, but it heavily depends on the browser. 

For example in chrome go to [chrome://net-internals/#hsts](chrome://net-internals/#hsts)
Here you can add a domain manually, delete a domain, and query the domain to see the header from that site.

## The include-subdomains directive

The scope of the HSTS header can be extended to sub-domains, protecting current and future sub-domains. You do need to be careful with this since it might block pages that don't use HTTPS. This is required for the preload directive.

## Trust On First Use (TOFU)

There is of course an obvious problem with this header, generally knows as **T**rust **O**n **F**irst **U**se, or **TOFU**. If the attacker can intercept the first interaction, he (or she) can easily strip the header from the response. 

## The preload directive

To prevent this window of opportunity, you can register your domain (with all subdomains). Browser builders will then add your domain to a list embedded in the browser, which will automatically use HTTPS for your domain. **[Registering your domain](https://hstspreload.org/#removal) will make it very hard to switch back to HTTP.**

To register your domain go to [https://hstspreload.appspot.com](https://hstspreload.appspot.com); this site belongs to the **chromium** project. It will take some time for your domain to be added to all browsers.

If you want to check if a site is on the **preload list**: here is the [list](https://cs.chromium.org/chromium/src/net/http/transport_security_state_static.json)

## Browser compatibility

Goto [http://caniuse.com/#feat=stricttransportsecurity](http://caniuse.com/#feat=stricttransportsecurity) to check compatibility.
Internet explorer is the unfortunate exception in this list, no support until IE 11...

## Adding the HSTS header to your website hosted in IIS

Adding the header is easy when your website is deployed with **IIS**. 
Simply add this in configuration:

```
<system.webServer>
  <customHeaders>
    <add name="Strict-Transport-Security" 
         value="max-age=6000; includeSubdomains;" />
  </customHeaders>
</system.webServer>
```

## Adding the HSTS header in .NET Core

You can easily add the header using the `U2U.AspNetCore.Security.Headers` package.

After adding the package add this to the `Configure` method in `Startup`

```
app.UseResponseHeaders(builder =>
{
  builder.SetStrictTransportSecurity(new StrictTransportSecurity
  {
    MaxAge = TimeSpan.FromDays(1),
    IncludeSubdomains = true,
    Preload = false
  });
};
```

You can add any header with this middleware using the `builder.SetHeader("Header", "Value")` method. But it also contains a couple of convenience methods such as `SetStrictTransportSecurity`.

---



