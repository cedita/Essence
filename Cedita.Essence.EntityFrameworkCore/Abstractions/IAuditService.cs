// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cedita.Essence.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Audit Service for EntityFramework Core.
    /// </summary>
    /// <typeparam name="TContext">Context Type.</typeparam>
    /// <typeparam name="TKey">Database Key Type.</typeparam>
    /// <typeparam name="TAuditEntity">Audit Entry used in DbContext.</typeparam>
    public interface IAuditService
    {
        /// <summary>
        /// Automatically audit changes against an entity in to the database context.
        /// </summary>
        /// <typeparam name="TEntity">Entity Type that this Audit belongs to.</typeparam>
        /// <param name="entry">Entry Entity that this Audit belongs to, to be scanned for changes.</param>
        /// <param name="metadata">Optional Metadata.</param>
        void ContextAddAuditChanges<TEntity>(EntityEntry<TEntity> entry, string metadata = null)
            where TEntity : class;

        /// <summary>
        /// Add an Audit with type Comment to the database context.
        /// </summary>
        /// <typeparam name="TEntity">Entity Type that this Audit belongs to.</typeparam>
        /// <param name="entry">Entry Entity that this Audit belongs to.</param>
        /// <param name="note">Note to be entered as a comment.</param>
        /// <param name="metadata">Optional Metadata.</param>
        void ContextAddComment<TEntity>(EntityEntry<TEntity> entry, string note, string metadata = null)
            where TEntity : class;

        /// <summary>
        /// Add an Audit with type Log to the database context.
        /// </summary>
        /// <typeparam name="TEntity">Entity Type that this Audit belongs to.</typeparam>
        /// <param name="entry">Entry Entity that this Audit belongs to.</param>
        /// <param name="note">Note to be entered as a log.</param>
        /// <param name="metadata">Optional Metadata.</param>
        void ContextAddLog<TEntity>(EntityEntry<TEntity> entry, string note, string metadata = null)
            where TEntity : class;
    }
}
