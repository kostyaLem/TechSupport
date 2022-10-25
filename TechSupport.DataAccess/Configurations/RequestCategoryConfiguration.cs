using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechSupport.DataAccess.Models;

namespace TechSupport.DataAccess.Configurations;

public class RequestCategoryConfiguration : IEntityTypeConfiguration<RequestCategory>
{
    public void Configure(EntityTypeBuilder<RequestCategory> builder)
    {
        builder.HasIndex(x => x.Title);
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description).IsRequired(false);
        builder.Property(x => x.ImageData).IsRequired(false);
    }
}
