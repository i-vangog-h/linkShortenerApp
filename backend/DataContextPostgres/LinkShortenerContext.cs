using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using linkApi.Entities;

namespace linkApi.DataContext;

public partial class LinkShortenerContext : DbContext
{

    public LinkShortenerContext()
    {
    }

    public LinkShortenerContext(DbContextOptions<LinkShortenerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Url> Urls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("en_US.UTF-8");

        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("url_pkey");

            entity.ToTable("url");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessCount)
                .HasDefaultValue(0)
                .HasColumnName("access_count");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Hash)
                .HasMaxLength(10)
                .HasColumnName("hash");
            entity.Property(e => e.OriginalUrl).HasColumnName("original_url");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
