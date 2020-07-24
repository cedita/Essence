In initial migration and any subsequent migrations where new tables have been added, add the following code to the Up() method for generating triggers and populating the Tables table.

    var dateModifiedSqls = DateModifiedTriggerGenerator.GetTriggersSql();
    foreach(var dateModifiedSql in dateModifiedSqls)
    {
        migrationBuilder.Sql(dateModifiedSql);
    }
    migrationBuilder.Sql(StaticSql.TableTable);


If you have non-standard table or class names, you can override the table names as passed in to the Trigger generator.

    var dateModifiedSqls = DateModifiedTriggerGenerator.GetTriggersSql(new Dictionary<Type, string> {
        { typeof(ThisIsNotStandard), "NonStandard" }
    });
