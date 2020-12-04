// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;

namespace Cedita.Essence.EntityFrameworkCore.Audit
{
    /// <summary>
    /// Entity Audit Interface.
    /// </summary>
    /// <typeparam name="TKey">Database Key.</typeparam>
    public interface IEntityAudit<TUserKey>
        where TUserKey : IComparable<TUserKey>
    {
        /// <summary>
        /// Gets or sets entity ID.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Gets or sets table Name of related Entity.
        /// </summary>
        string Table { get; set; }

        /// <summary>
        /// Gets or sets navigation to Table for referential integrity.
        /// </summary>
        Table TableNav { get; set; }

        /// <summary>
        /// Gets or sets table ID (Primary Key) of related Entity.
        /// </summary>
        TUserKey TableId { get; set; }

        /// <summary>
        /// Gets or sets user ID (Primary Key) of User.
        /// </summary>
        TUserKey UserId { get; set; }

        /// <summary>
        /// Gets or sets type of Audit.
        /// </summary>
        AuditType Type { get; set; }

        /// <summary>
        /// Gets or sets date of which the Audit occurred.
        /// </summary>
        DateTimeOffset DateAudit { get; set; }

        /// <summary>
        /// Gets or sets column Name for Field Audits.
        /// </summary>
        string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets previous Value (From) of Audit.
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// Gets or sets changed Value (To) of Audit.
        /// </summary>
        string To { get; set; }

        /// <summary>
        /// Gets or sets note against this Audit.
        /// </summary>
        string Note { get; set; }

        /// <summary>
        /// Gets or sets additional metadata (user defined).
        /// </summary>
        string Metadata { get; set; }
    }
}
