
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

#nullable disable


public class BloggingContext : DbContext
{

    public BloggingContext(DbContextOptions<BloggingContext> options, Action<BloggingContext, ModelBuilder> modelCustomizer = null)
        : base(options)
    {
    }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    public DbSet<Blog> Blogs => Set<Blog>();
    //    public DbSet<UrlResource> UrlResources => Set<UrlResource>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var DbPath = "./Blogging.db";
            optionsBuilder.UseSqlite($"DbPath={DbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //        modelBuilder.Entity<UrlResource>().HasNoKey()
        //            .ToView("AllResources");
    }

}

public class Blog
{
    public int BlogId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
}


public class UrlResource
{
    public string Url { get; set; }
}
