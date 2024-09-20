using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductAPI.Repository.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            builder.HasData(
                new IdentityUser
                {
                    Id = "1",
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    SecurityStamp = string.Empty
                },
                new IdentityUser
                {
                    Id = "2",
                    UserName = "user@example.com",
                    NormalizedUserName = "USER@EXAMPLE.COM",
                    Email = "user@example.com",
                    NormalizedEmail = "USER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "User123!"),
                    SecurityStamp = string.Empty
                }
            );
        }
    }
}
