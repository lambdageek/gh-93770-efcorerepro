using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;


using var dbController = new SqliteInMemoryBloggingControllerTest();

using var context = dbController.CreateContext();


// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database: {context.Database.GetDbConnection().ConnectionString}");

var db = context.Database;

// Create
Console.WriteLine("Inserting a new blog");
context.Blogs.Add(new Blog { Name="Hello World", Url = "http://blahblahblah"});
context.SaveChanges();

// Read
Console.WriteLine("Querying for a blog");
var blog = context.Blogs.FirstOrDefault(b => b.Name == "Hello World");
Console.WriteLine (blog.Url);

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
context.SaveChanges();

Console.WriteLine("Querying for a blog again");
blog = context.Blogs.FirstOrDefault(b => b.Name == "Hello World");
Console.WriteLine (blog.Url);


// Delete
// Console.WriteLine("Delete the blog");
//db.Remove(blog);
//db.SaveChanges();