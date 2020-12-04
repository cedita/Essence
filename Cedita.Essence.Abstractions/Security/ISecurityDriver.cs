// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;

namespace Cedita.Essence.Abstractions.Security
{
    public interface ISecurityDriver
    {
        /// <summary>
        /// Check if the currently logged in User has a specific Claim of type <paramref name="claimType"/>.
        /// </summary>
        /// <param name="claimType">Claim Type</param>
        /// <returns>True if claim exists; false otherwise.</returns>
        bool HasClaim(string claimType);

        /// <summary>
        /// Get the value of a Claim of type <paramref name="claimType"/> from the currently logged in User.
        /// </summary>
        /// <param name="claimType">Claim Type</param>
        /// <typeparam name="TResult">Type of Claim Value.</typeparam>
        /// <returns>Value of Claim, mapped to specific type.</returns>
        TResult GetClaimValue<TResult>(string claimType);

        /// <summary>
        /// Add or Update a specific Claim of the currently logged in User.
        /// </summary>
        /// <param name="claimType">Claim Type</param>
        /// <param name="claimValue">Claim Value</param>
        Task AddOrUpdateClaimAsync(string claimType, string claimValue);
    }
}
