using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cedita.Essence.Abstractions.Security;
using Cedita.Essence.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cedita.Essence.AspNetCore.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IApiKeyProvider apiKeyProvider;
        private readonly ILogger<ApiKeyAuthenticationHandler> logger;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyProvider apiKeyProvider)
            : base(options, logger, encoder, clock)
        {
            this.apiKeyProvider = apiKeyProvider;
            this.logger = logger.CreateLogger<ApiKeyAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string apiKey = default;

            if (Options.EnableHeaderAuthentication)
            {
                // First try setting from the header if enabled
                if (Request.Headers.TryGetValue(Options.ApiKeyHeader, out var apiKeyHeader))
                {
                    apiKey = apiKeyHeader.FirstOrDefault();
                }
            }

            if (apiKey.IsNullOrWhitespace() && Options.EnableQueryStringAuthentication)
            {
                if (Request.Query.TryGetValue(Options.ApiKeyQueryString, out var apiKeyParameter))
                {
                    apiKey = apiKeyParameter.FirstOrDefault();
                }
            }

            if (apiKey.IsNullOrWhitespace())
            {
                logger.LogDebug("Could not retrieve API Key from header or query (as configured).");
                return AuthenticateResult.NoResult();
            }

            var actualApiKey = await apiKeyProvider.GetApiKeyAsync(apiKey);

            if (actualApiKey == null)
            {
                logger.LogDebug($"API Key {apiKey} was not valid.");
                return AuthenticateResult.Fail("Invalid API Key.");
            }

            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, actualApiKey.UserId),
            };

            if (actualApiKey.AdditionalClaims != null && actualApiKey.AdditionalClaims.Any())
            {
                claims = claims.Concat(actualApiKey.AdditionalClaims);
            }

            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(
                    new List<ClaimsIdentity>
                    {
                        new ClaimsIdentity(
                            claims,
                            Options.Scheme,
                            Options.NameClaimType,
                            Options.RoleClaimType),
                    }), Options.Scheme);

            logger.LogDebug("API Key.");

            return AuthenticateResult.Success(authenticationTicket);
        }
    }
}
