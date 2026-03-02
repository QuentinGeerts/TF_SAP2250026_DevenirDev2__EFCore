using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCore.Configurations;

public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.ToTable("Directors");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Lastname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Firstname)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new Director
            {
                Id = new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"),
                Lastname = "Cameron",
                Firstname = "James"
            }, 
            new Director
            {
                Id = new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"),
                Lastname = "Geerts",
                Firstname = "Quentin"
            }
        );
    }
}
