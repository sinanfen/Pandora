using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePasswordExpirationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("604feb50-ece3-4b4c-9f2d-33d61245943c"), new Guid("593a6bb4-3ecd-49e5-8af0-474d65f13f6e") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a41ec15a-e04f-461a-8a48-13bd933b86fb"), new Guid("83c4a9ce-a96a-49d2-9c10-39ceff3f8b45") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("604feb50-ece3-4b4c-9f2d-33d61245943c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a41ec15a-e04f-461a-8a48-13bd933b86fb"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("593a6bb4-3ecd-49e5-8af0-474d65f13f6e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("83c4a9ce-a96a-49d2-9c10-39ceff3f8b45"));

            migrationBuilder.DropColumn(
                name: "PasswordExpirationDate",
                table: "PasswordVaults");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpirationDate",
                table: "PasswordVaults",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("604feb50-ece3-4b4c-9f2d-33d61245943c"), new DateTime(2025, 6, 27, 23, 44, 42, 204, DateTimeKind.Utc).AddTicks(2381), null, "Admin", "ADMIN", null },
                    { new Guid("a41ec15a-e04f-461a-8a48-13bd933b86fb"), new DateTime(2025, 6, 27, 23, 44, 42, 204, DateTimeKind.Utc).AddTicks(2391), null, "User", "USER", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("593a6bb4-3ecd-49e5-8af0-474d65f13f6e"), new DateTime(2025, 6, 27, 23, 44, 42, 204, DateTimeKind.Utc).AddTicks(5582), null, "admin@example.com", true, "Admin", new DateTime(2025, 6, 27, 23, 44, 42, 204, DateTimeKind.Utc).AddTicks(6036), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "08dc05cf-2888-45b4-b607-09d83fc2eed6", false, null, "admin" },
                    { new Guid("83c4a9ce-a96a-49d2-9c10-39ceff3f8b45"), new DateTime(2025, 6, 27, 23, 44, 42, 204, DateTimeKind.Utc).AddTicks(6039), null, "user@example.com", true, "User", new DateTime(2025, 6, 27, 23, 44, 42, 204, DateTimeKind.Utc).AddTicks(6088), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "9cb64608-bc0c-44c3-bec4-167e791c6a5c", false, null, "user" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("604feb50-ece3-4b4c-9f2d-33d61245943c"), new Guid("593a6bb4-3ecd-49e5-8af0-474d65f13f6e") },
                    { new Guid("a41ec15a-e04f-461a-8a48-13bd933b86fb"), new Guid("83c4a9ce-a96a-49d2-9c10-39ceff3f8b45") }
                });
        }
    }
}
