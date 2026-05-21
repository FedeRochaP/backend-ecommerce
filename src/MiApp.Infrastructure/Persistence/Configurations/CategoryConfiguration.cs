using MiApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiApp.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new { Id = new Guid("a1b2c3d4-0000-0000-0000-000000000001"), Name = "Electrónica" },
            new { Id = new Guid("a1b2c3d4-0000-0000-0000-000000000002"), Name = "Ropa" },
            new { Id = new Guid("a1b2c3d4-0000-0000-0000-000000000003"), Name = "Hogar" }
        );
    }
}
