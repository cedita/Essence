namespace Cedita.Essence.EntityFrameworkCore.SqlServer
{
    public static class StaticSql
    {
        public const string TableTable = @"
INSERT INTO [dbo].[Tables] ([Name])
SELECT [TABLE_NAME] FROM [INFORMATION_SCHEMA].[TABLES] st
WHERE [TABLE_NAME] <> '_MigrationHistory' AND NOT EXISTS (SELECT Name FROM [dbo].[Tables] t WHERE t.Name = st.TABLE_NAME)";
    }
}
