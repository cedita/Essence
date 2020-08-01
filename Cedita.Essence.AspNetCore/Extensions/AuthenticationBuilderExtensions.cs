using System;
using Cedita.Essence.AspNetCore.Security;
using Microsoft.AspNetCore.Authentication;

namespace Cedita.Essence.AspNetCore.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Add the API Key Authentication Handler to the Authentication pipeline.
        /// </summary>
        /// <param name="authenticationBuilder">Builder.</param>
        /// <param name="options">Used to configure the API Key Authentication options.</param>
        /// <returns>The builder.</returns>
        public static AuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options = null)
        {
            if (options == null)
            {
                options = o => { };
            }

            return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.SchemeName, options);
        }
    }
}
