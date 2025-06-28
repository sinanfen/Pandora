using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeCapsuleFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("0a171143-f7fd-4193-a59c-20ce9488f5d7"), new Guid("4ee0dd59-5877-4aeb-8b35-2b83149617f7") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6493d2f0-4096-4256-b8ea-10a6722b6581"), new Guid("c4998627-3244-4d0c-84fb-1440710529ce") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0a171143-f7fd-4193-a59c-20ce9488f5d7"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6493d2f0-4096-4256-b8ea-10a6722b6581"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4ee0dd59-5877-4aeb-8b35-2b83149617f7"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c4998627-3244-4d0c-84fb-1440710529ce"));

            migrationBuilder.AddColumn<bool>(
                name: "IsShareable",
                table: "PersonalVaults",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShareToken",
                table: "PersonalVaults",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShareViewCount",
                table: "PersonalVaults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SharedAt",
                table: "PersonalVaults",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("bc97ab7b-00fb-4f27-9b99-edf58305a4e8"), new DateTime(2025, 6, 28, 17, 43, 6, 843, DateTimeKind.Utc).AddTicks(2583), null, "Admin", "ADMIN", null },
                    { new Guid("ca4b53b5-4e94-423d-9149-9121c31e230c"), new DateTime(2025, 6, 28, 17, 43, 6, 843, DateTimeKind.Utc).AddTicks(2634), null, "User", "USER", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("89b3d6ca-d362-4397-98cf-255d0904d32a"), new DateTime(2025, 6, 28, 17, 43, 6, 843, DateTimeKind.Utc).AddTicks(5923), null, "admin@example.com", true, "Admin", new DateTime(2025, 6, 28, 17, 43, 6, 843, DateTimeKind.Utc).AddTicks(6595), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "d9b631fa-5bea-4683-b09b-359f571cf140", false, null, "admin" },
                    { new Guid("b93c7537-b729-4748-91b3-fd25875f52db"), new DateTime(2025, 6, 28, 17, 43, 6, 843, DateTimeKind.Utc).AddTicks(6597), null, "user@example.com", true, "User", new DateTime(2025, 6, 28, 17, 43, 6, 843, DateTimeKind.Utc).AddTicks(6643), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "9e8a2176-869c-41c6-9461-72d78b9319ae", false, null, "user" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("bc97ab7b-00fb-4f27-9b99-edf58305a4e8"), new Guid("89b3d6ca-d362-4397-98cf-255d0904d32a") },
                    { new Guid("ca4b53b5-4e94-423d-9149-9121c31e230c"), new Guid("b93c7537-b729-4748-91b3-fd25875f52db") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("bc97ab7b-00fb-4f27-9b99-edf58305a4e8"), new Guid("89b3d6ca-d362-4397-98cf-255d0904d32a") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("ca4b53b5-4e94-423d-9149-9121c31e230c"), new Guid("b93c7537-b729-4748-91b3-fd25875f52db") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bc97ab7b-00fb-4f27-9b99-edf58305a4e8"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ca4b53b5-4e94-423d-9149-9121c31e230c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("89b3d6ca-d362-4397-98cf-255d0904d32a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b93c7537-b729-4748-91b3-fd25875f52db"));

            migrationBuilder.DropColumn(
                name: "IsShareable",
                table: "PersonalVaults");

            migrationBuilder.DropColumn(
                name: "ShareToken",
                table: "PersonalVaults");

            migrationBuilder.DropColumn(
                name: "ShareViewCount",
                table: "PersonalVaults");

            migrationBuilder.DropColumn(
                name: "SharedAt",
                table: "PersonalVaults");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0a171143-f7fd-4193-a59c-20ce9488f5d7"), new DateTime(2025, 6, 28, 16, 18, 55, 430, DateTimeKind.Utc).AddTicks(3462), null, "User", "USER", null },
                    { new Guid("6493d2f0-4096-4256-b8ea-10a6722b6581"), new DateTime(2025, 6, 28, 16, 18, 55, 430, DateTimeKind.Utc).AddTicks(3450), null, "Admin", "ADMIN", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("4ee0dd59-5877-4aeb-8b35-2b83149617f7"), new DateTime(2025, 6, 28, 16, 18, 55, 430, DateTimeKind.Utc).AddTicks(7588), null, "user@example.com", true, "User", new DateTime(2025, 6, 28, 16, 18, 55, 430, DateTimeKind.Utc).AddTicks(7637), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "2ad656aa-76f1-4ed0-862d-a7403600d0be", false, null, "user" },
                    { new Guid("c4998627-3244-4d0c-84fb-1440710529ce"), new DateTime(2025, 6, 28, 16, 18, 55, 430, DateTimeKind.Utc).AddTicks(7095), null, "admin@example.com", true, "Admin", new DateTime(2025, 6, 28, 16, 18, 55, 430, DateTimeKind.Utc).AddTicks(7586), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "6b55cf32-5062-46e2-b94d-3313faaec9b8", false, null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("0a171143-f7fd-4193-a59c-20ce9488f5d7"), new Guid("4ee0dd59-5877-4aeb-8b35-2b83149617f7") },
                    { new Guid("6493d2f0-4096-4256-b8ea-10a6722b6581"), new Guid("c4998627-3244-4d0c-84fb-1440710529ce") }
                });
        }
    }
}
