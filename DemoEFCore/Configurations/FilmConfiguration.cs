using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCore.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        // Configuration de la table
        builder.ToTable("Films", schema =>
        {
            schema.HasCheckConstraint("CK_Film_ReleasedYear_After_1500", "ReleasedYear >= 1500");
        });

        // Configuration des colonnes
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.ReleasedYear)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        // Remplissage de la base de données (Seed Data)
        builder.HasData(
            new Film { 
                Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000001"), 
                Title = "The Shawshank Redemption", 
                ReleasedYear = 1994, 
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) 
            },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000002"), Title = "The Godfather", ReleasedYear = 1972, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000003"), Title = "The Dark Knight", ReleasedYear = 2008, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000004"), Title = "Pulp Fiction", ReleasedYear = 1994, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000005"), Title = "Schindler's List", ReleasedYear = 1993, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000006"), Title = "The Lord of the Rings: The Return of the King", ReleasedYear = 2003, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000007"), Title = "Forrest Gump", ReleasedYear = 1994, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000008"), Title = "Inception", ReleasedYear = 2010, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000009"), Title = "Fight Club", ReleasedYear = 1999, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000010"), Title = "Goodfellas", ReleasedYear = 1990, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000011"), Title = "The Matrix", ReleasedYear = 1999, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000012"), Title = "Interstellar", ReleasedYear = 2014, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000013"), Title = "Se7en", ReleasedYear = 1995, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000014"), Title = "The Silence of the Lambs", ReleasedYear = 1991, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000015"), Title = "Saving Private Ryan", ReleasedYear = 1998, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000016"), Title = "Gladiator", ReleasedYear = 2000, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000017"), Title = "The Prestige", ReleasedYear = 2006, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000018"), Title = "Whiplash", ReleasedYear = 2014, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000019"), Title = "Parasite", ReleasedYear = 2019, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Film { Id = new Guid("a1b2c3d4-e5f6-7890-abcd-000000000020"), Title = "Oppenheimer", ReleasedYear = 2023, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
