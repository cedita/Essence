// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;

namespace Cedita.Essence.Abstractions.Security
{
    /// <summary>
    /// Implement to provide standardised API Keys for Cedita.Essence.AspNetCore security.
    /// </summary>
    public interface IApiKeyProvider
    {
        /// <summary>
        /// Given an API Key, return the IApiKey structure.
        /// </summary>
        /// <param name="key">Key to find.</param>
        /// <returns>API Key, or null if not valid.</returns>
        Task<IApiKey> GetApiKeyAsync(string key);
    }
}
