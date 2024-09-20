
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductAPI.Core.Models;

namespace ProductAPI.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = "1",
                    Name = "Laptop",
                    Description = "Powerful laptop for developers",
                    Price = 1299.99m,
                    StockQuantity = 50,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = "2",
                    Name = "Smartphone",
                    Description = "Latest model smartphone",
                    Price = 799.99m,
                    StockQuantity = 100,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = "3",
                    Name = "Headphones",
                    Description = "Noise-cancelling wireless headphones",
                    Price = 199.99m,
                    StockQuantity = 200,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}

