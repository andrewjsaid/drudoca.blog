namespace Drudoca.Blog.DataAccess.Sql;

internal class DatabaseOptions
{
    public string SqliteConnectionString { get; set; } = "Data Source=:memory:";
}