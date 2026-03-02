using DemoEFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoEFCore.Configurations;

public class UserDetailsConfiguration : IEntityTypeConfiguration<UserDetails>
{
    public void Configure(EntityTypeBuilder<UserDetails> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Lastname).HasMaxLength(50);
        builder.Property(x => x.Firstname).HasMaxLength(50);        
    }
}
