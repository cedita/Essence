// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Authentication;

namespace Cedita.Essence.AspNetCore
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string SchemeName = "API Key";

        public string Scheme => SchemeName;

        /// <summary>
        /// Gets or sets the Header Name if used.
        /// </summary>
        public string ApiKeyHeader { get; set; } = "X-Api-Key";

        /// <summary>
        /// Gets or sets the Query String Parameter Name if used.
        /// </summary>
        public string ApiKeyQueryString { get; set; } = "apiKey";

        /// <summary>
        /// Gets or Sets the Claim Type for User Name when creating ClaimsIdentity.
        /// </summary>
        public string NameClaimType { get; set; } = "name";

        /// <summary>
        /// Gets or Sets the Claim Type for Role(s) when creating ClaimsIdentity.
        /// </summary>
        public string RoleClaimType { get; set; } = "role";

        /// <summary>
        /// Gets or sets a value indicating whether to enable API Key Authentication by a Header..
        /// </summary>
        public bool EnableHeaderAuthentication { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to enable API Key Authentication by a Query String parameter.
        /// </summary>
        public bool EnableQueryStringAuthentication { get; set; } = false;
    }
}
