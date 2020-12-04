using System;
using System.Collections.Concurrent;
using Cedita.Essence.Abstractions.Security;
using Cedita.Essence.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cedita.Essence.EntityFrameworkCore.Audit
{
    /// <summary>
    /// Audit Service to automatically audit changes on an entity within the <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">Context Type.</typeparam>
    /// <typeparam name="TUserKey">Database Key Type.</typeparam>
    /// <typeparam name="TAuditEntity">Audit Entry used in DbContext.</typeparam>
    public class AuditService<TContext, TUserKey, TAuditEntity> : IAuditService
        where TContext : DbContext
        where TUserKey : IComparable<TUserKey>
        where TAuditEntity : class, IEntityAudit<TUserKey>, new()
    {
        private readonly DbSet<TAuditEntity> dbSet;

        /// <summary>
        /// Initialises a new instance of the <see cref="AuditService{TContext, TKey, TAuditEntity}"/> class.
        /// </summary>
        /// <param name="context">Database Context.</param>
        /// <param name="userIdProvider">User ID Provider.</param>
        public AuditService(TContext context, IUserIdProvider userIdProvider)
        {
            this.Context = context;
            this.UserIdProvider = userIdProvider;
            this.dbSet = context.Set<TAuditEntity>();
        }

        /// <summary>
        /// Gets database Context of type <typeparamref name="TContext"/>.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <summary>
        /// Gets user ID Provider for getting UserID from implementor.
        /// </summary>
        protected IUserIdProvider UserIdProvider { get; private set; }

        /// <summary>
        /// Gets the Cache of Types to Table Names.
        /// </summary>
        protected ConcurrentDictionary<Type, string> TableNames { get; } = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Gets the Cache of Types to Primary Key Column Names.
        /// </summary>
        protected ConcurrentDictionary<Type, string> TablePks { get; } = new ConcurrentDictionary<Type, string>();

        /// <inheritdoc/>
        public void ContextAddAuditChanges<TEntity>(EntityEntry<TEntity> entry, string metadata = null)
            where TEntity : class
        {
            var tableName = GetTableName(typeof(TEntity));
            var primaryKey = (TUserKey)entry.CurrentValues[GetPrimaryKeyColumnName(typeof(TEntity))];
            var auditDate = DateTimeOffset.Now;

            object userId = null;
            if (UserIdProvider.HasCurrentUser())
            {
                userId = UserIdProvider.GetCurrentUserId();
            }

            foreach (var changedProp in entry.CurrentValues.Properties)
            {
                var currentValue = entry.CurrentValues[changedProp];
                var originalValue = entry.OriginalValues[changedProp];

                if (
                    (currentValue != null || originalValue != null) &&
                    (
                        (currentValue != null && originalValue == null) ||
                        (currentValue == null && originalValue != null) ||
                        !currentValue.Equals(originalValue)))
                {
                    dbSet.Add(new TAuditEntity
                    {
                        Table = tableName,
                        TableId = primaryKey,
                        Type = AuditType.Audit,
                        UserId = (TUserKey)userId,
                        ColumnName = changedProp.GetColumnName(),
                        From = originalValue?.ToString(),
                        To = currentValue?.ToString(),
                        DateAudit = auditDate,
                        Metadata = metadata,
                    });
                }
            }
        }

        /// <inheritdoc/>
        public void ContextAddComment<TEntity>(EntityEntry<TEntity> entry, string note, string metadata = null)
            where TEntity : class
        {
            var tableName = GetTableName(typeof(TEntity));
            var primaryKey = (TUserKey)entry.CurrentValues[GetPrimaryKeyColumnName(typeof(TEntity))];

            object userId = null;
            if (UserIdProvider.HasCurrentUser())
            {
                userId = (TUserKey)UserIdProvider.GetCurrentUserId();
            }

            dbSet.Add(new TAuditEntity
            {
                Table = tableName,
                TableId = primaryKey,
                Type = AuditType.Comment,
                UserId = (TUserKey)userId,
                Note = note,
                DateAudit = DateTimeOffset.Now,
                Metadata = metadata,
            });
        }

        /// <inheritdoc/>
        public void ContextAddLog<TEntity>(EntityEntry<TEntity> entry, string note, string metadata = null)
            where TEntity : class
        {
            var tableName = GetTableName(typeof(TEntity));
            var primaryKey = (TUserKey)entry.CurrentValues[GetPrimaryKeyColumnName(typeof(TEntity))];

            object userId = null;
            if (UserIdProvider.HasCurrentUser())
            {
                userId = (TUserKey)UserIdProvider.GetCurrentUserId();
            }

            dbSet.Add(new TAuditEntity
            {
                Table = tableName,
                TableId = primaryKey,
                Type = AuditType.Log,
                UserId = (TUserKey)userId,
                Note = note,
                DateAudit = DateTimeOffset.Now,
                Metadata = metadata,
            });
        }

        /// <summary>
        /// Get the PTable Name for an Entity Type.
        /// </summary>
        /// <param name="type">Entity Type.</param>
        /// <returns>Table Name.</returns>
        protected string GetTableName(Type type)
        {
            var cacheHit = TableNames.TryGetValue(type, out string tableName);

            if (!cacheHit)
            {
                tableName = Context.Model.FindEntityType(type).GetTableName();

                TableNames.TryAdd(type, tableName);
            }

            return tableName;
        }

        /// <summary>
        /// Get the Primary Key Column Name for an Entity Type.
        /// </summary>
        /// <param name="type">Entity Type.</param>
        /// <returns>Primary Key Column Name.</returns>
        protected string GetPrimaryKeyColumnName(Type type)
        {
            var cacheHit = TablePks.TryGetValue(type, out string pkColumnName);

            if (!cacheHit)
            {
                pkColumnName = Context.Model.FindEntityType(type).FindPrimaryKey().GetName();

                TablePks.TryAdd(type, pkColumnName);
            }

            return pkColumnName;
        }
    }
}
