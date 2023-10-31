using Microsoft.EntityFrameworkCore;

public class SqliteInMemoryBloggingControllerTest : IDisposable
{
    //private readonly DbConnection _connection;
    private readonly DbContextOptions<BloggingContext> _contextOptions;

    public SqliteInMemoryBloggingControllerTest()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        //_connection = new SqliteConnection("Filename=:memory:");
        //_connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<BloggingContext>()
            //.UseSqlite(_connection)
            .UseInMemoryDatabase("test")
            .Options;

        // Create the schema and seed some data
        var context = new BloggingContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            Console.WriteLine("created the database from the model");
        }
        // context.Dispose();
    }

    public void Dispose() { } // => _connection.Dispose();
}