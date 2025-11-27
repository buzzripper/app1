namespace BffMicrosoftEntraID.Server;

public static class SecurityHeadersDefinitions
{
    private static HeaderPolicyCollection? policy;

    public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev, string? idpHost)
    {
        ArgumentNullException.ThrowIfNull(idpHost);

        // Avoid building a new HeaderPolicyCollection on every request for performance reasons.
        // Where possible, cache and reuse HeaderPolicyCollection instances.
        if (policy != null) return policy;

        policy = new HeaderPolicyCollection()
            .AddFrameOptionsDeny()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .AddCrossOriginOpenerPolicy(builder => builder.SameOrigin())
            .AddCrossOriginResourcePolicy(builder => builder.SameOrigin())
            .AddCrossOriginEmbedderPolicy(builder => builder.RequireCorp()) // remove for dev if using hot reload
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().None();
                builder.AddBlockAllMixedContent();
                builder.AddImgSrc().Self().From("data:");
                builder.AddFormAction().Self().From(idpHost);
                // Allow fonts from self and Google Fonts
                builder.AddFontSrc().Self().From("https://fonts.gstatic.com");
                builder.AddBaseUri().Self();
                builder.AddFrameAncestors().None();

                if (isDev)
                {
                    // Allow styles from self, Google Fonts, and inline styles
                    builder.AddStyleSrc()
                        .Self()
                        .From("https://fonts.googleapis.com")
                        .UnsafeInline();
                    
                    // In development, allow ASP.NET Core dev tools (BrowserLink, hot reload)
                    builder.AddScriptSrcElem()
                        .Self()
                        .WithNonce()
                        .UnsafeInline();
                    builder.AddScriptSrc()
                        .Self()
                        .WithNonce()
                        .UnsafeInline();
                }
                else
                {
                    // Production: Use nonce-based CSP
                    builder.AddStyleSrc()
                        .Self()
                        .From("https://fonts.googleapis.com")
                        .WithNonce()
                        .UnsafeInline();
                    
                    builder.AddScriptSrcElem().WithNonce().UnsafeInline();
                    builder.AddScriptSrc().WithNonce().UnsafeInline();
                }
            })
            .RemoveServerHeader()
            .AddPermissionsPolicyWithDefaultSecureDirectives();

        if (!isDev)
        {
            // maxage = one year in seconds
            policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365);
        }

        return policy;
    }
}