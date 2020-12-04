using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cedita.Essence.EntityFrameworkCore.Audit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cedita.Essence.EntityFrameworkCore.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configure Essence auditable entities (IAuditDates) with correct column specification.
        /// </summary>
        /// <param name="builder">Model Builder.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureAuditableEntities(this ModelBuilder builder)
        {
            var type = typeof(IAuditDates);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);
            foreach (var concreteType in types)
            {
                builder.Entity(concreteType, e =>
                {
                    e.Property(nameof(IAuditDates.DateCreated)).HasDefaultValueSql("SYSDATETIMEOFFSET()").ValueGeneratedOnAdd();
                    e.Property(nameof(IAuditDates.DateModified)).HasDefaultValueSql("SYSDATETIMEOFFSET()").ValueGeneratedOnAddOrUpdate();
                });
            }

            return builder;
        }

        /// <summary>
        /// Configure default decimal column precision.
        /// </summary>
        /// <param name="builder">Model Builder.</param>
        /// <param name="precision">Decimal Precision.</param>
        /// <param name="scale">Decimal Scale.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureDecimalPrecision(this ModelBuilder builder, int precision = 19, int scale = 4)
        {
            return builder.ConfigureDefaultTypeToColumn(typeof(decimal), $"decimal({precision}, {scale})");
        }

        /// <summary>
        /// Configure default Column type of Type.
        /// </summary>
        /// <param name="builder">Model Builder.</param>
        /// <param name="type">Type to configure.</param>
        /// <param name="columnType">Column type to configure.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureDefaultTypeToColumn(this ModelBuilder builder, Type type, string columnType)
        {
            // Is this a value type that we can also configure Nullable<T> for?
            var extraType = default(Type);
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                extraType = typeof(Nullable<>).MakeGenericType(type);
            }

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == type || (extraType != null && p.ClrType == extraType)))
            {
                if (property.GetColumnType() == null)
                {
                    property.SetColumnType(columnType);
                }
            }

            return builder;
        }

        /// <summary>
        /// Configure Delete Behaviour across all keys on types that are not excluded.
        /// </summary>
        /// <param name="builder">Model Builder.</param>
        /// <param name="deleteBehavior">Delete Behaviour to configure.</param>
        /// <param name="excludedTypes">Types to exclude from change.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureDeleteBehaviour(this ModelBuilder builder, DeleteBehavior deleteBehavior, params Type[] excludedTypes)
        {
            var excludedEntities = new List<IMutableEntityType>();

            foreach (var excludedType in excludedTypes)
            {
                excludedEntities.Add(builder.Model.FindEntityType(excludedType));
            }

            foreach (var relationship in builder.Model.GetEntityTypes().Except(excludedEntities).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            return builder;
        }

        /// <summary>
        /// Configure Delete Behaviour across all keys on types that are not excluded, adding default Identity tables to the exclusion list.
        /// </summary>
        /// <typeparam name="TUser">Identity User Type.</typeparam>
        /// <typeparam name="TKey">Identity Key type.</typeparam>
        /// <param name="builder">Model Builder.</param>
        /// <param name="deleteBehavior">Delete Behaviour to configure.</param>
        /// <param name="excludedTypes">Types to exclude from change in addition to Identity types.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureDeleteBehaviourExcludingIdentity<TUser, TKey>(this ModelBuilder builder, DeleteBehavior deleteBehavior, params Type[] excludedTypes)
            where TKey : IEquatable<TKey>
        {
            excludedTypes = excludedTypes.Concat(new[] {
                typeof(TUser),
                typeof(IdentityRole<TKey>),
                typeof(IdentityUserRole<TKey>),
                typeof(IdentityUserClaim<TKey>),
                typeof(IdentityUserLogin<TKey>),
                typeof(IdentityUserToken<TKey>),
                typeof(IdentityRoleClaim<TKey>),
            }).ToArray();

            return ConfigureDeleteBehaviour(builder, deleteBehavior, excludedTypes);
        }

        /// <summary>
        /// Configure Query Filter on all types that are not excluded to deny access.
        /// </summary>
        /// <param name="builder">Model Builder.</param>
        /// <param name="excludedTypes">Types to exclude from change.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureDenyQueryFilter(this ModelBuilder builder, params Type[] excludedTypes)
        {
            foreach (var entityType in builder.Model.GetEntityTypes().Select(m => m.ClrType).Except(excludedTypes))
            {
                builder.Entity(entityType, a => a.HasQueryFilter(CreateDenyFilterForType(entityType)));
            }

            return builder;
        }

        private static LambdaExpression CreateDenyFilterForType(Type type)
        {
            var returnTarget = Expression.Label(typeof(bool));
            var param = Expression.Parameter(type, "m");
            return Expression.Lambda(Expression.Return(returnTarget, Expression.Constant(false)), param);
        }

        /// <summary>
        /// Configure Query Filter on all types that are not excluded to deny access, adding default Identity tables to the exclusion list.
        /// </summary>
        /// <typeparam name="TUser">Identity User Type.</typeparam>
        /// <typeparam name="TKey">Identity Key type.</typeparam>
        /// <param name="builder">Model Builder.</param>
        /// <param name="excludedTypes">Types to exclude from change in addition to Identity types.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder ConfigureDenyQueryFilterExcludingIdentity<TUser, TKey>(this ModelBuilder builder, params Type[] excludedTypes)
            where TKey : IEquatable<TKey>
        {
            excludedTypes = excludedTypes.Concat(new[] {
                typeof(TUser),
                typeof(IdentityRole<TKey>),
                typeof(IdentityUserRole<TKey>),
                typeof(IdentityUserClaim<TKey>),
                typeof(IdentityUserLogin<TKey>),
                typeof(IdentityUserToken<TKey>),
                typeof(IdentityRoleClaim<TKey>),
            }).ToArray();

            return ConfigureDenyQueryFilter(builder, excludedTypes);
        }
    }
}
