// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cedita.Essence.EntityFrameworkCore.Audit
{
    /// <summary>
    /// EntityAudit type for EF Core configured.
    /// </summary>
    /// <typeparam name="TUserKey">Default Key type of tables.</typeparam>
    /// <typeparam name="TUser">User Entity Type for link table (eg. IdentityUser).</typeparam>
    public class EntityAudit<TUserKey, TUser> : IEntityAudit<TUserKey>
        where TUserKey : IComparable<TUserKey>
    {
        /// <summary>
        /// Gets or sets entity ID.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets table Name of related Entity.
        /// </summary>
        [MaxLength(128)]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets navigation to Table for referential integrity.
        /// </summary>
        public Table TableNav { get; set; }

        /// <summary>
        /// Gets or sets table ID (Primary Key) of related Entity.
        /// </summary>
        public TUserKey TableId { get; set; }

        /// <summary>
        /// Gets or sets navigation to User for referential integrity.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public TUser User { get; set; }

        /// <summary>
        /// Gets or sets user ID (Primary Key) of User.
        /// </summary>
        public TUserKey UserId { get; set; }

        /// <summary>
        /// Gets or sets type of Audit.
        /// </summary>
        public AuditType Type { get; set; }

        /// <summary>
        /// Gets or sets date of which the Audit occurred.
        /// </summary>
        public DateTimeOffset DateAudit { get; set; }

        /// <summary>
        /// Gets or sets column Name for Field Audits.
        /// </summary>
        [MaxLength(128)]
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets previous Value (From) of Audit.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets changed Value (To) of Audit.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets note against this Audit.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets additional metadata (user defined).
        /// </summary>
        public string Metadata { get; set; }
    }
}
