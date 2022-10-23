using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSupport.DataAccess.Models;

namespace TechSupport.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName)
            .IsRequired();
        builder.Property(x => x.Birthday)
            .IsRequired();
        builder.HasIndex(x => new { x.FirstName, x.LastName, x.Phone });
    }
}
