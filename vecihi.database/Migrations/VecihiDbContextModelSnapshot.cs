﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vecihi.database;

namespace vecihi.database.Migrations
{
    [DbContext(typeof(VecihiDbContext))]
    partial class VecihiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("vecihi.database.model.AutoCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CodeFormat")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedBy");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("LastCodeNumber");

                    b.Property<string>("ScreenCode")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("AutoCode");
                });

            modelBuilder.Entity("vecihi.database.model.AutoCodeLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AutoCodeId");

                    b.Property<DateTime>("CodeGenerationDate");

                    b.Property<int>("CodeNumber");

                    b.Property<Guid>("GeneratedBy");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.HasIndex("AutoCodeId");

                    b.HasIndex("GeneratedBy");

                    b.ToTable("AutoCodeLog");
                });

            modelBuilder.Entity("vecihi.database.model.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedBy");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasMaxLength(20);

                    b.Property<string>("Title");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0c5337a5-ca82-4c97-94e9-00101a1d749d"),
                            CreatedAt = new DateTime(2019, 9, 4, 23, 17, 0, 352, DateTimeKind.Local).AddTicks(1689),
                            CreatedBy = new Guid("7cbf9971-7957-48dd-8198-3394a9bf0059"),
                            IsDeleted = false,
                            Name = "qnill",
                            Title = "Back-end Developer",
                            UserId = new Guid("7cbf9971-7957-48dd-8198-3394a9bf0059")
                        });
                });

            modelBuilder.Entity("vecihi.database.model.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedBy");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<Guid>("RefId");

                    b.Property<string>("ScreenCode")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<long?>("Size");

                    b.Property<string>("StorageName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("vecihi.database.model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<DateTime?>("LastLoginDate");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7cbf9971-7957-48dd-8198-3394a9bf0059"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "00f2abb4-b297-448c-8081-7e1d8820d5ae",
                            Email = "qnill@foo.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "QNILL@FOO.COM",
                            NormalizedUserName = "QNILL",
                            PasswordHash = "AQAAAAEAACcQAAAAEI9wRRam6GzQR3oK1v5U+CqjKoj8Xx7d89hEQmbJZ6yoAFeiXYbTRNpgL2spaCgU6w==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ec8870d4-9eca-4ff7-a741-2b4e4970a41a",
                            TwoFactorEnabled = false,
                            UserName = "qnill"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("vecihi.database.model.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("vecihi.database.model.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("vecihi.database.model.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("vecihi.database.model.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("vecihi.database.model.AutoCodeLog", b =>
                {
                    b.HasOne("vecihi.database.model.AutoCode", "AutoCode")
                        .WithMany("AutoCodeLogs")
                        .HasForeignKey("AutoCodeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("vecihi.database.model.User", "User")
                        .WithMany("AutoCodeLogs")
                        .HasForeignKey("GeneratedBy")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("vecihi.database.model.Employee", b =>
                {
                    b.HasOne("vecihi.database.model.User", "User")
                        .WithMany("Employees")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
