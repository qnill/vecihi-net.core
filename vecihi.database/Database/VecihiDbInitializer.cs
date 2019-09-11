using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using vecihi.database.model;

namespace vecihi.database
{
    public static class VecihiDbInitializer
    {
        public static void Seed(this ModelBuilder builder)
        {
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("7cbf9971-7957-48dd-8198-3394a9bf0059"),
                    UserName = "qnill",
                    NormalizedUserName = "QNILL",
                    Email = "qnill@foo.com",
                    NormalizedEmail = "QNILL@FOO.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Test123*"),
                    SecurityStamp = string.Empty,
                    ConcurrencyStamp = string.Empty
                });

            builder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.Parse("0c5337a5-ca82-4c97-94e9-00101a1d749d"),
                    Title = "Back-end Developer",
                    Name = "qnill",
                    UserId = Guid.Parse("7cbf9971-7957-48dd-8198-3394a9bf0059"),
                    CreatedAt = new DateTime(2019, 9, 1),
                    CreatedBy = Guid.Parse("7cbf9971-7957-48dd-8198-3394a9bf0059")
                });
        }
    }
}