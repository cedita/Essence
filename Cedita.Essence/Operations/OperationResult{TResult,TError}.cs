// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Linq;

namespace Cedita.Essence.Operations
{
    /// <summary>
    /// Represents the result of an operation with a specified Result and Error type.
    /// </summary>
    /// <typeparam name="TResult">Type of Result.</typeparam>
    /// <typeparam name="TError">Type of Error.</typeparam>
    public class OperationResult<TResult, TError> : OperationResult<TResult>
    {
        /// <summary>
        /// Gets or sets the error of the Operation.
        /// </summary>
        public TError Error { get; set; }

        /// <summary>
        /// Create an instance of <see cref="OperationResult{TResult}"/> representing a failed operation with a list of
        /// <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="error">Typed Error.</param>
        /// <param name="errors">An optional array of <see cref="OperationError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="OperationResult{TResult}"/> indicating a failed operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static OperationResult<TResult, TError> Failure(TError error, params OperationError[] errors)
        {
            var result = new OperationResult<TResult, TError> { Succeeded = false, Error = error };
            if (errors != null)
            {
                result.errors.AddRange(errors);
            }

            return result;
        }

        /// <summary>
        /// Create an instance of <see cref="OperationResult{TResult,TError}"/> representing a failed operation with a list of
        /// <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="error">Typed Error.</param>
        /// <param name="errors">A list of <see cref="OperationError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="OperationResult{TResult}"/> indicating a failed operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static OperationResult<TResult, TError> Failure(TError error, IEnumerable<OperationError> errors)
        {
            return new OperationResult<TResult, TError>
            {
                Succeeded = false,
                Error = error,
                errors = errors.ToList(),
            };
        }

        /// <summary>
        /// Create an instance of <see cref="OperationResult{TResult}"/> representing a successful operation with a result value.
        /// </summary>
        /// <param name="value">Result value.</param>
        /// <returns>An <see cref="OperationResult{TResult}"/> indicating a successful operation, with a result value.</returns>
        public static new OperationResult<TResult, TError> Success(TResult value)
        {
            return new OperationResult<TResult, TError>
            {
                Succeeded = true,
                Result = value,
            };
        }
    }
}
