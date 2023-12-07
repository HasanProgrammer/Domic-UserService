using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karami.Persistence.Migrations.C
{
    public partial class CreateTablesVersion_1_0_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissionUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PermissionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt_EnglishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt_PersianDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionUsers_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissionUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUsers_PermissionId",
                table: "PermissionUsers",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUsers_UserId",
                table: "PermissionUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUsers_RoleId",
                table: "RoleUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUsers_UserId",
                table: "RoleUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "PermissionUsers");

            migrationBuilder.DropTable(
                name: "RoleUsers");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
