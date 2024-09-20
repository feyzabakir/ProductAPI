using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Core.Models;

namespace ProductAPI.Repository.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // GUID olarak Id'yi ayarlıyoruz ve veritabanında otomatik olarak oluşturulmasını sağlıyoruz
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(36)  // GUID uzunluğu
                .ValueGeneratedOnAdd();  // Id'nin otomatik olarak oluşturulmasını sağlıyoruz

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.StockQuantity).IsRequired();

            builder.ToTable("Products");
        }
    }
}

