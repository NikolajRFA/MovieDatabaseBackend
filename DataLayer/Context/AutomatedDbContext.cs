using System;
using System.Collections.Generic;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Context;

public partial class AutomatedDbContext : DbContext
{
    public AutomatedDbContext()
    {
    }

    public AutomatedDbContext(DbContextOptions<AutomatedDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alias> Aliases { get; set; }

    public virtual DbSet<Bookmark> Bookmarks { get; set; }

    public virtual DbSet<Crew> Crews { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<IsEpisodeOf> IsEpisodeOfs { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<Rated> Rateds { get; set; }

    public virtual DbSet<Search> Searches { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wi> Wis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
#warning    You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - 
#warning    see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=cit.ruc.dk;db=cit06;uid=cit06;pwd=sTF6Cwwe1qXG");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alias>(entity =>
        {
            entity.HasKey(e => new { e.Tconst, e.Ordering }).HasName("aliases_pkey");

            entity.ToTable("aliases");

            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Attributes)
                .HasMaxLength(256)
                .HasColumnName("attributes");
            entity.Property(e => e.Isoriginaltitle).HasColumnName("isoriginaltitle");
            entity.Property(e => e.Language)
                .HasMaxLength(10)
                .HasColumnName("language");
            entity.Property(e => e.Region)
                .HasMaxLength(10)
                .HasColumnName("region");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Types)
                .HasMaxLength(256)
                .HasColumnName("types");

            entity.HasOne(d => d.TconstNavigation).WithMany(p => p.Aliases)
                .HasForeignKey(d => d.Tconst)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aliases_tconst_fkey");
        });

        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookmark_pkey");

            entity.ToTable("bookmark");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("nconst");
            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.NconstNavigation).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.Nconst)
                .HasConstraintName("bookmark_nconst_fkey");

            entity.HasOne(d => d.TconstNavigation).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.Tconst)
                .HasConstraintName("bookmark_tconst_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookmark_userid_fkey");
        });

        modelBuilder.Entity<Crew>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crew_pkey");

            entity.ToTable("crew");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Character)
                .HasMaxLength(256)
                .HasColumnName("character");
            entity.Property(e => e.Job).HasColumnName("job");
            entity.Property(e => e.Nconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("nconst");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");

            entity.HasOne(d => d.NconstNavigation).WithMany(p => p.Crews)
                .HasForeignKey(d => d.Nconst)
                .HasConstraintName("crew_nconst_fkey");

            entity.HasOne(d => d.TconstNavigation).WithMany(p => p.Crews)
                .HasForeignKey(d => d.Tconst)
                .HasConstraintName("crew_tconst_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Genre1)
                .HasMaxLength(50)
                .HasColumnName("genre");

            entity.HasMany(d => d.Tconsts).WithMany(p => p.Ids)
                .UsingEntity<Dictionary<string, object>>(
                    "HasGenre",
                    r => r.HasOne<Title>().WithMany()
                        .HasForeignKey("Tconst")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("has_genre_tconst_fkey"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("has_genre_id_fkey"),
                    j =>
                    {
                        j.HasKey("Id", "Tconst").HasName("has_genre_pkey");
                        j.ToTable("has_genre");
                        j.IndexerProperty<int>("Id").HasColumnName("id");
                        j.IndexerProperty<string>("Tconst")
                            .HasMaxLength(10)
                            .IsFixedLength()
                            .HasColumnName("tconst");
                    });
        });

        modelBuilder.Entity<IsEpisodeOf>(entity =>
        {
            entity.HasKey(e => new { e.Tconst, e.Parenttconst }).HasName("is_episode_of_pkey");

            entity.ToTable("is_episode_of");

            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");
            entity.Property(e => e.Parenttconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("parenttconst");
            entity.Property(e => e.Episodenumber).HasColumnName("episodenumber");
            entity.Property(e => e.Seasonnumber).HasColumnName("seasonnumber");

            entity.HasOne(d => d.ParenttconstNavigation).WithMany(p => p.IsEpisodeOfParenttconstNavigations)
                .HasForeignKey(d => d.Parenttconst)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("is_episode_of_parenttconst_fkey");

            entity.HasOne(d => d.TconstNavigation).WithMany(p => p.IsEpisodeOfTconstNavigations)
                .HasForeignKey(d => d.Tconst)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("is_episode_of_tconst_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Nconst).HasName("persons_pkey");

            entity.ToTable("persons");

            entity.Property(e => e.Nconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("nconst");
            entity.Property(e => e.Birthyear)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("birthyear");
            entity.Property(e => e.Deathyear)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("deathyear");
            entity.Property(e => e.NameRating)
                .HasDefaultValueSql("0")
                .HasColumnName("name_rating");
            entity.Property(e => e.Personname)
                .HasMaxLength(256)
                .HasColumnName("personname");

            entity.HasMany(d => d.Professions).WithMany(p => p.Nconsts)
                .UsingEntity<Dictionary<string, object>>(
                    "HasProfession",
                    r => r.HasOne<Profession>().WithMany()
                        .HasForeignKey("Professionid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("has_profession_professionid_fkey"),
                    l => l.HasOne<Person>().WithMany()
                        .HasForeignKey("Nconst")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("has_profession_nconst_fkey"),
                    j =>
                    {
                        j.HasKey("Nconst", "Professionid").HasName("has_profession_pkey");
                        j.ToTable("has_profession");
                        j.IndexerProperty<string>("Nconst")
                            .HasMaxLength(10)
                            .IsFixedLength()
                            .HasColumnName("nconst");
                        j.IndexerProperty<int>("Professionid").HasColumnName("professionid");
                    });
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("professions_pkey");

            entity.ToTable("professions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Profession1)
                .HasMaxLength(50)
                .HasColumnName("profession");
        });

        modelBuilder.Entity<Rated>(entity =>
        {
            entity.HasKey(e => new { e.Tconst, e.Id }).HasName("rated_pkey");

            entity.ToTable("rated");

            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("rated_id_fkey");

            entity.HasOne(d => d.TconstNavigation).WithMany(p => p.Rateds)
                .HasForeignKey(d => d.Tconst)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rated_tconst_fkey");
        });

        modelBuilder.Entity<Search>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Searchphrase, e.Date }).HasName("searches_pkey");

            entity.ToTable("searches");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Searchphrase)
                .HasMaxLength(256)
                .HasColumnName("searchphrase");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Searches)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("searches_id_fkey");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.Tconst).HasName("titles_pkey");

            entity.ToTable("titles");

            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");
            entity.Property(e => e.Averagerating)
                .HasPrecision(5, 1)
                .HasColumnName("averagerating");
            entity.Property(e => e.Endyear)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("endyear");
            entity.Property(e => e.Isadult).HasColumnName("isadult");
            entity.Property(e => e.Numvotes).HasColumnName("numvotes");
            entity.Property(e => e.Originaltitle).HasColumnName("originaltitle");
            entity.Property(e => e.Plot).HasColumnName("plot");
            entity.Property(e => e.Poster)
                .HasMaxLength(180)
                .HasColumnName("poster");
            entity.Property(e => e.Primarytitle).HasColumnName("primarytitle");
            entity.Property(e => e.Runtimeminutes).HasColumnName("runtimeminutes");
            entity.Property(e => e.Startyear)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("startyear");
            entity.Property(e => e.Titletype)
                .HasMaxLength(120)
                .HasColumnName("titletype");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Wi>(entity =>
        {
            entity.HasKey(e => new { e.Tconst, e.Word, e.Field }).HasName("wi_pkey");

            entity.ToTable("wi");

            entity.Property(e => e.Tconst)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tconst");
            entity.Property(e => e.Word).HasColumnName("word");
            entity.Property(e => e.Field)
                .HasMaxLength(1)
                .HasColumnName("field");
            entity.Property(e => e.Lexeme).HasColumnName("lexeme");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}