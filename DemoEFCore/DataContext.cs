using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoEFCore;

public class DataContext : DbContext
{
    // DBSet : Table SQL => Collection C#
    public DbSet<Director> Directors { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<User> Users { get; set; }

    // OnConfiguring: Permet de configurer la connexion
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=FilmDb;Integrated Security=True;Trust Server Certificate=True";
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ajouter manuellement l'instance de la configuration
        //modelBuilder.ApplyConfiguration(new FilmConfiguration());

        // Ajouter automatiquement les configurations
        //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
