using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace vecihi.auth
{
    public static class AuthDbInitializer
    {
        public static void Seed(this ModelBuilder builder)
        {
            var hasher = new PasswordHasher<AuthUser>();

            builder.Entity<AuthUser>().HasData(
                new AuthUser
                {
                    Id = "7cbf9971-7957-48dd-8198-3394a9bf0059",
                    UserName = "qnill",
                    NormalizedUserName = "QNILL",
                    Email = "qnill@foo.com",
                    NormalizedEmail = "QNILL@FOO.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Test123*"),
                    SecurityStamp = Guid.NewGuid().ToString("D")
                });
        }
    }
}