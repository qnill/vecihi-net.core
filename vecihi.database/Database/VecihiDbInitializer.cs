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
            Guid userId = Guid.Parse("7cbf9971-7957-48dd-8198-3394a9bf0059");
            Guid employeeId = Guid.Parse("0c5337a5-ca82-4c97-94e9-00101a1d749d");

            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = userId,
                    Name = "qnill",
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
                    Id = employeeId,
                    Title = "Back-end Developer",
                    Name = "qnill",
                    UserId = userId,
                    CreatedAt = new DateTime(2019, 9, 1),
                    CreatedBy = userId
                });
        }
    }
}