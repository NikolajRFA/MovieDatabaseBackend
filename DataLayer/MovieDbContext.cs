using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class MovieDbContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Crew> Crew { get; set; }
    public DbSet<Alias> Aliases { get; set; }
    public DbSet<IsEpisodeOf> IsEpisodeOf { get; set; }

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
        modelBuilder.Entity<Title>().ToTable("titles").HasKey(x => new { x.Tconst });

        modelBuilder.Entity<Person>().ToTable("persons").HasKey(x => new { x.Nconst });

        modelBuilder.Entity<Alias>().ToTable("aliases").HasKey(x => new { x.Tconst, x.Ordering });

        modelBuilder.Entity<IsEpisodeOf>().ToTable("is_episode_of").HasKey(x => new { x.Tconst, x.ParentTconst });
    }
}