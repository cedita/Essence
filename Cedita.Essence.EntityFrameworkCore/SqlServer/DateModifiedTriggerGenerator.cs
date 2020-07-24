using System;
using System.Collections.Generic;
using System.Linq;
using Cedita.Essence.EntityFrameworkCore.Audit;
using Cedita.Essence.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Cedita.Essence.EntityFrameworkCore
{
    public static class DateModifiedTriggerGenerator
    {
        /// <summary>
        /// Get Trigger Creation T-SQL to be used in migrations.
        /// </summary>
        /// <param name="tableNameOverrides">(Optional) Override types to specific table names.</param>
        /// <returns>List of SQL to execute to create triggers if they do not exist.</returns>
        public static IEnumerable<string> GetTriggersSql(Dictionary<Type, string> tableNameOverrides = null)
        {
            var type = typeof(IAuditDates);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);

            var sqlOutput = new List<string>();

            foreach (var concreteType in types)
            {
                var name = concreteType.Name;
                if (tableNameOverrides != null && tableNameOverrides.ContainsKey(concreteType))
                {
                    name = tableNameOverrides[concreteType];
                }
                else
                {
                    // Let's test for IdentityUser which is by default mapped to AspNetUsers table
                    if (type.IsAssignableFromGenericType(typeof(IdentityUser<>)))
                    {
                        name = "AspNetUser";
                    }

                    switch (name.Substring(name.Length - 1))
                    {
                        case "s":
                            name += "es";
                            break;
                        case "y":
                            name = name[0..^1] + "ies";
                            break;
                        default:
                            name += "s";
                            break;
                    }
                }

                sqlOutput.Add(
$@"
IF OBJECT_ID ('[{name}_UPD_DM]', 'TR') IS NOT NULL
   DROP TRIGGER [{name}_UPD_DM];
GO
CREATE TRIGGER [dbo].[{name}_UPD_DM] ON [dbo].[{name}]
    AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

    UPDATE dbo.{name}
    SET DateModified = SYSDATETIMEOFFSET()
    WHERE Id IN (SELECT DISTINCT ID FROM Inserted)
END");
            }

            return sqlOutput;
        }
    }
}
