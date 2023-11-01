using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class MovieDbContext : DbContext
{
    // Movie database
    public DbSet<Title> Titles { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Crew> Crew { get; set; }
    public DbSet<Alias> Aliases { get; set; }
    public DbSet<IsEpisodeOf> IsEpisodeOf { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<HasProfession> HasProfessions { get; set; }
    public DbSet<Wi> Wi { get; set; }


    // Framework database
    public DbSet<User> Users { get; set; }
    public DbSet<Search> Searches { get; set; }
    public DbSet<Rating> Rated { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<BestMatch> BestMatches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder
            .LogTo(Console.Out.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=cit.ruc.dk;db=cit06;uid=cit06;pwd=sTF6Cwwe1qXG");
        optionsBuilder.UseLowerCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Movie database
        modelBuilder.Entity<Title>().ToTable("titles").HasKey(x => new { x.Tconst });

        modelBuilder.Entity<HasProfession>().ToTable("has_profession")
            .HasKey(x => new { x.Nconst, x.ProfessionId });

        modelBuilder.Entity<Profession>().Property(x => x.ProfessionName).HasColumnName("profession");

        modelBuilder.Entity<Person>().ToTable("persons").HasKey(x => new { x.Nconst });
        modelBuilder.Entity<Person>().Property(x => x.NameRating).HasColumnName("name_rating");
        modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("personname");
        modelBuilder.Entity<Person>()
            .HasMany(x => x.Professions)
            .WithMany(x => x.Person)
            .UsingEntity<HasProfession>();

        modelBuilder.Entity<Genre>().Property(x => x.GenreName).HasColumnName("genre");

        modelBuilder.Entity<HasGenre>().ToTable("has_genre")
            .HasKey(x => new { x.Id, x.Tconst });
        modelBuilder.Entity<HasGenre>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<Title>()
            .HasMany(x => x.Genre)
            .WithMany(x => x.Title)
            .UsingEntity<HasGenre>()
            .HasOne(x => x.Genre)
            .WithMany()
            .HasForeignKey(x => x.Id);

        modelBuilder.Entity<Crew>()
            .HasOne(x => x.Title)
            .WithMany(x => x.Crew)
            .HasForeignKey(x => x.Tconst);
        modelBuilder.Entity<Crew>()
            .HasOne(x => x.Person)
            .WithMany(x => x.Crews)
            .HasForeignKey(x => x.Nconst);
        
        modelBuilder.Entity<Alias>().ToTable("aliases").HasKey(x => new { x.Tconst, x.Ordering });

        modelBuilder.Entity<IsEpisodeOf>().ToTable("is_episode_of").HasKey(x => new { x.Tconst, x.ParentTconst });

        modelBuilder.Entity<Wi>().ToTable("wi").HasKey(x => new { x.Tconst, x.Word, x.Field });

        modelBuilder.Entity<BestMatch>().HasNoKey();
        
        // Framework database
        modelBuilder.Entity<Search>().ToTable("searches").HasKey(x => new { x.Id, x.SearchPhrase, x.Date });

        modelBuilder.Entity<User>()
            .HasMany(x => x.Searches)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.Id);
        modelBuilder.Entity<User>()
            .HasMany(x => x.Ratings)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.Id);

        modelBuilder.Entity<Rating>().ToTable("rated").HasKey(x => new { x.Tconst, x.Id });
        modelBuilder.Entity<Rating>().Property(x => x.ThisRating).HasColumnName("rating");

        modelBuilder.Entity<Bookmark>().ToTable("bookmark");
    }
}