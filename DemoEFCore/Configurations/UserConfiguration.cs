using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCore.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", schema =>
        {
            schema.HasCheckConstraint("CK_Users_Email_Format", "Email LIKE '_%@_%._%'");
        });

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(u => u.Lastname)
            .HasMaxLength(50);

        builder.Property(u => u.Firstname)
            .HasMaxLength(50);
    }
}
