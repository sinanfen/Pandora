using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactorSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorBackupCodes",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFactorEnabledAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecretKey",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("a97e48d0-0d5a-47a6-b032-7f84f282c42a"), new DateTime(2025, 6, 28, 18, 19, 56, 227, DateTimeKind.Utc).AddTicks(7558), null, "User", "USER", null },
                    { new Guid("dc7bbe32-07ac-4f2c-88ba-f81abddf2979"), new DateTime(2025, 6, 28, 18, 19, 56, 227, DateTimeKind.Utc).AddTicks(7545), null, "Admin", "ADMIN", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorBackupCodes", "TwoFactorEnabled", "TwoFactorEnabledAt", "TwoFactorSecretKey", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("0fb0e9db-77ab-490d-baf0-a5f7dbd76622"), new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(1510), null, "admin@example.com", true, "Admin", new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(1978), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "b188f86b-85f8-4fa7-9324-874459d68fb3", null, false, null, null, null, "admin" },
                    { new Guid("ce80c84a-6885-45d5-9a45-2ebb129537c4"), new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(1981), null, "user@example.com", true, "User", new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(2036), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "806e5b53-4cb4-4502-a268-5d6124c1a7ea", null, false, null, null, null, "user" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("dc7bbe32-07ac-4f2c-88ba-f81abddf2979"), new Guid("0fb0e9db-77ab-490d-baf0-a5f7dbd76622") },
                    { new Guid("a97e48d0-0d5a-47a6-b032-7f84f282c42a"), new Guid("ce80c84a-6885-45d5-9a45-2ebb129537c4") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dc7bbe32-07ac-4f2c-88ba-f81abddf2979"), new Guid("0fb0e9db-77ab-490d-baf0-a5f7dbd76622") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a97e48d0-0d5a-47a6-b032-7f84f282c42a"), new Guid("ce80c84a-6885-45d5-9a45-2ebb129537c4") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a97e48d0-0d5a-47a6-b032-7f84f282c42a"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dc7bbe32-07ac-4f2c-88ba-f81abddf2979"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0fb0e9db-77ab-490d-baf0-a5f7dbd76622"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ce80c84a-6885-45d5-9a45-2ebb129537c4"));

            migrationBuilder.DropColumn(
                name: "TwoFactorBackupCodes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabledAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKey",
                table: "Users");

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
    }
}
