using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vecihi.database.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    ScreenCode = table.Column<string>(maxLength: 5, nullable: false),
                    CodeFormat = table.Column<string>(maxLength: 20, nullable: false),
                    LastCodeNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    RefId = table.Column<Guid>(nullable: false),
                    ScreenCode = table.Column<string>(maxLength: 5, nullable: false),
                    OriginalName = table.Column<string>(maxLength: 30, nullable: false),
                    StorageName = table.Column<string>(maxLength: 50, nullable: false),
                    Extension = table.Column<string>(maxLength: 10, nullable: false),
                    Size = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoCodeLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CodeNumber = table.Column<int>(nullable: false),
                    CodeGenerationDate = table.Column<DateTime>(nullable: false),
                    GeneratedBy = table.Column<Guid>(nullable: false),
                    AutoCodeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoCodeLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoCodeLog_AutoCode_AutoCodeId",
                        column: x => x.AutoCodeId,
                        principalTable: "AutoCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "IsDeleted", "LastLoginDate", "Name", "Phone", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { new Guid("0c5337a5-ca82-4c97-94e9-00101a1d749d"), new DateTime(2019, 9, 1, 16, 13, 24, 575, DateTimeKind.Local).AddTicks(9953), new Guid("7cbf9971-7957-48dd-8198-3394a9bf0059"), null, false, null, "qnill", null, null, null, new Guid("7cbf9971-7957-48dd-8198-3394a9bf0059") });

            migrationBuilder.CreateIndex(
                name: "IX_AutoCodeLog_AutoCodeId",
                table: "AutoCodeLog",
                column: "AutoCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoCodeLog");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "AutoCode");
        }
    }
}
