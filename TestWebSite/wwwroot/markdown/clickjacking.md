## Preventing the attack

**Click Jacking** can be prevented with the `Content Security Policy` header.

Use the frame-ancesters to allow content sources that can embed this page.

If you don't want anyone to frame your site use content source 'none'.

```
Content-Security-Policy: ...; frame-ancestors 'none';
```
