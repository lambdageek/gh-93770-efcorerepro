using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class SqliteInMemoryBloggingControllerTest : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<BloggingContext> _contextOptions;

    public SqliteInMemoryBloggingControllerTest()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new BloggingContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            Console.WriteLine ("created the database from the model");
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT Url
FROM Blogs;";
            viewCommand.ExecuteNonQuery();
        }

        context.AddRange(
            new Blog { Name = "Blog1", Url = "http://blog1.com" },
            new Blog { Name = "Blog2", Url = "http://blog2.com" });
        context.SaveChanges();
    }

    public BloggingContext CreateContext() => new BloggingContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
}