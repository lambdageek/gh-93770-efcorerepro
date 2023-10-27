using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Identity;

#nullable disable


public class BloggingContext : DbContext
{
    private readonly Action<BloggingContext, ModelBuilder> _modelCustomizer;


    public BloggingContext(DbContextOptions<BloggingContext> options, Action<BloggingContext, ModelBuilder> modelCustomizer = null)
        : base(options)
    {
        _modelCustomizer = modelCustomizer;
    }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<UrlResource> UrlResources => Set<UrlResource>();

#if false
    public DbSet<IdentityUser> AspNetUsers => Set<IdentityUser>();

    public DbSet<IdentityUserClaim<string>> AspNetUserClaims => Set<IdentityUserClaim<string>>();

    public DbSet<IdentityUserLogin<string>> AspNetUserLogins => Set<IdentityUserLogin<string>>();

    public DbSet<IdentityUserToken<string>> AspNetUserTokens => Set<IdentityUserToken<string>>();
#endif

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
        modelBuilder.Entity<UrlResource>().HasNoKey()
            .ToView("AllResources");
        // OnModelCreatingVersion1(modelBuilder);

        if (_modelCustomizer is not null)
        {
            _modelCustomizer(this, modelBuilder);
        }
    }

#if false
    internal virtual void OnModelCreatingVersion1(ModelBuilder builder)
    {
        //var storeOptions = GetStoreOptions();
        var maxKeyLength = 0; // storeOptions?.MaxLengthForKeys ?? 0;

        builder.Entity<IdentityUser>(b =>
        {
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
            b.ToTable(nameof(AspNetUsers));
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.NormalizedUserName).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);
            b.Property(u => u.NormalizedEmail).HasMaxLength(256);


            b.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        });

        builder.Entity<IdentityUserClaim<string>>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable(nameof(AspNetUserClaims));
        });

        builder.Entity<IdentityUserLogin<string>>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            if (maxKeyLength > 0)
            {
                b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
            }

            b.ToTable(nameof(AspNetUserLogins));
        });

        builder.Entity<IdentityUserToken<string>>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            if (maxKeyLength > 0)
            {
                b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                b.Property(t => t.Name).HasMaxLength(maxKeyLength);
            }

            b.ToTable(nameof(AspNetUserTokens));
        });
    }
#endif
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

public interface IBloggingRepository
{
    Blog GetBlogByName(string name);

    IEnumerable<Blog> GetAllBlogs();

    void AddBlog(Blog blog);

    void SaveChanges();
}
public class BloggingRepository : IBloggingRepository
{
    private readonly BloggingContext _context;

    public BloggingRepository(BloggingContext context)
        => _context = context;

    public Blog GetBlogByName(string name)
        => _context.Blogs.FirstOrDefault(b => b.Name == name);

    public IEnumerable<Blog> GetAllBlogs()
        => _context.Blogs;

    public void AddBlog(Blog blog)
        => _context.Add(blog);

    public void SaveChanges()
        => _context.SaveChanges();
}