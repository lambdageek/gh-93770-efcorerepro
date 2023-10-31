
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

#nullable disable


public class BloggingContext : DbContext
{

    public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("DbContextOptionsBuilder is not configured.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}

public class Blog
{
    public int BlogId { get; set; }
    //    public string Name { get; set; }
    //    public string Url { get; set; }
}
