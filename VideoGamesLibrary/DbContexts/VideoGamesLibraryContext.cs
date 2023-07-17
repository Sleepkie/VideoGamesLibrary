using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VideoGamesLibrary.Models;

namespace VideoGamesLibrary.DbContexts;

public partial class VideoGamesLibraryContext : DbContext
{
    public VideoGamesLibraryContext()
    {
    }

    public VideoGamesLibraryContext(DbContextOptions<VideoGamesLibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("developers_pkey");

            entity.ToTable("developers");

            entity.HasIndex(e => e.Name, "developers_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_pkey");

            entity.ToTable("games");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Developerid).HasColumnName("developerid");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            entity.HasOne(d => d.Developer).WithMany(p => p.Games)
                .HasForeignKey(d => d.Developerid)
                .HasConstraintName("games_developerid_fkey");

            entity.HasMany(d => d.Genres).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "Gamegenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("Genreid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("gamegenres_genreid_fkey"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("Gameid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("gamegenres_gameid_fkey"),
                    j =>
                    {
                        j.HasKey("Gameid", "Genreid").HasName("gamegenres_pkey");
                        j.ToTable("gamegenres");
                        j.IndexerProperty<int>("Gameid").HasColumnName("gameid");
                        j.IndexerProperty<int>("Genreid").HasColumnName("genreid");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.Name, "genres_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
