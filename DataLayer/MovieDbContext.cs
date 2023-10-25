using DataLayer.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class MovieDbContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    
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
        modelBuilder.Entity<Title>().ToTable("titles").HasKey(x=>new{x.Tconst});

    }
}