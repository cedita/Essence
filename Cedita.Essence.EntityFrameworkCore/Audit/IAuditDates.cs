using System;

namespace Cedita.Essence.EntityFrameworkCore.Audit
{
    public interface IAuditDates
    {
        DateTimeOffset DateCreated { get; set; }

        DateTimeOffset DateModified { get; set; }

        DateTimeOffset? DateDeleted { get; set; }
    }
}
