// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Linq;

namespace Cedita.Essence.Operations
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    /// <typeparam name="TResult">Type of Result.</typeparam>
    public class OperationResult<TResult> : OperationResult
    {
        /// <summary>
        /// Gets or sets the result of the Operation.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Create an instance of <see cref="OperationResult{TResult}"/> representing a failed operation with a list of
        /// <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="OperationError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="OperationResult{TResult}"/> indicating a failed operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static new OperationResult<TResult> Failure(params OperationError[] errors)
        {
            var result = new OperationResult<TResult> { Succeeded = false };
            if (errors != null)
            {
                result.errors.AddRange(errors);
            }

            return result;
        }

        /// <summary>
        /// Create an instance of <see cref="OperationResult{TResult}"/> representing a failed operation with a list of
        /// <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">A list of <see cref="OperationError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="OperationResult{TResult}"/> indicating a failed operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static new OperationResult<TResult> Failure(IEnumerable<OperationError> errors)
        {
            return new OperationResult<TResult>
            {
                Succeeded = false,
                errors = errors.ToList(),
            };
        }

        /// <summary>
        /// Create an instance of <see cref="OperationResult{TResult}"/> representing a successful operation with a result value.
        /// </summary>
        /// <param name="value">Result value.</param>
        /// <returns>An <see cref="OperationResult{TResult}"/> indicating a successful operation, with a result value.</returns>
        public static new OperationResult<TResult> Success(TResult value)
        {
            return new OperationResult<TResult>
            {
                Succeeded = true,
                Result = value,
            };
        }
    }
}
