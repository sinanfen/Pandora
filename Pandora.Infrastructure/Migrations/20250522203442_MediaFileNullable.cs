using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MediaFileNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("eb69b15c-fcf8-44a0-98e3-9630a130052e"), new Guid("77a77e5c-3bcf-42bc-bc11-b6c604d55353") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f9a3d333-ef36-4f94-b79f-aa040b538343"), new Guid("a3bef7de-3bc1-4682-8f25-23232fc4b21b") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("eb69b15c-fcf8-44a0-98e3-9630a130052e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f9a3d333-ef36-4f94-b79f-aa040b538343"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("77a77e5c-3bcf-42bc-bc11-b6c604d55353"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3bef7de-3bc1-4682-8f25-23232fc4b21b"));

            migrationBuilder.AlterColumn<string>(
                name: "SecureMediaFile",
                table: "PersonalVaults",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("d07ed317-6155-4c6c-b894-7ca115e222d2"), new DateTime(2025, 5, 22, 20, 34, 42, 299, DateTimeKind.Utc).AddTicks(2448), null, "Admin", "ADMIN", null },
                    { new Guid("d9a9054b-c1df-47d7-bb30-3deb5f728ac5"), new DateTime(2025, 5, 22, 20, 34, 42, 299, DateTimeKind.Utc).AddTicks(2454), null, "User", "USER", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("3db4a5d6-fe8c-446c-858b-741f7d4eecaf"), new DateTime(2025, 5, 22, 20, 34, 42, 299, DateTimeKind.Utc).AddTicks(7493), null, "user@example.com", true, "User", new DateTime(2025, 5, 22, 20, 34, 42, 299, DateTimeKind.Utc).AddTicks(7549), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "450451e2-d797-425e-b3df-184b40f2db99", false, null, "user" },
                    { new Guid("5f9c2315-e878-420d-932c-48db1aa78592"), new DateTime(2025, 5, 22, 20, 34, 42, 299, DateTimeKind.Utc).AddTicks(7122), null, "admin@example.com", true, "Admin", new DateTime(2025, 5, 22, 20, 34, 42, 299, DateTimeKind.Utc).AddTicks(7491), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "ef28492f-92cd-45ac-90ba-0ab704830f96", false, null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("d9a9054b-c1df-47d7-bb30-3deb5f728ac5"), new Guid("3db4a5d6-fe8c-446c-858b-741f7d4eecaf") },
                    { new Guid("d07ed317-6155-4c6c-b894-7ca115e222d2"), new Guid("5f9c2315-e878-420d-932c-48db1aa78592") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d9a9054b-c1df-47d7-bb30-3deb5f728ac5"), new Guid("3db4a5d6-fe8c-446c-858b-741f7d4eecaf") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d07ed317-6155-4c6c-b894-7ca115e222d2"), new Guid("5f9c2315-e878-420d-932c-48db1aa78592") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d07ed317-6155-4c6c-b894-7ca115e222d2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d9a9054b-c1df-47d7-bb30-3deb5f728ac5"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3db4a5d6-fe8c-446c-858b-741f7d4eecaf"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5f9c2315-e878-420d-932c-48db1aa78592"));

            migrationBuilder.AlterColumn<string>(
                name: "SecureMediaFile",
                table: "PersonalVaults",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("eb69b15c-fcf8-44a0-98e3-9630a130052e"), new DateTime(2025, 3, 29, 14, 39, 30, 650, DateTimeKind.Utc).AddTicks(253), null, "Admin", "ADMIN", null },
                    { new Guid("f9a3d333-ef36-4f94-b79f-aa040b538343"), new DateTime(2025, 3, 29, 14, 39, 30, 650, DateTimeKind.Utc).AddTicks(258), null, "User", "USER", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("77a77e5c-3bcf-42bc-bc11-b6c604d55353"), new DateTime(2025, 3, 29, 14, 39, 30, 650, DateTimeKind.Utc).AddTicks(3418), null, "admin@example.com", true, "Admin", new DateTime(2025, 3, 29, 14, 39, 30, 650, DateTimeKind.Utc).AddTicks(3768), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "aa18b1ff-212e-47a9-a2f7-1cd7805dbadd", false, null, "admin" },
                    { new Guid("a3bef7de-3bc1-4682-8f25-23232fc4b21b"), new DateTime(2025, 3, 29, 14, 39, 30, 650, DateTimeKind.Utc).AddTicks(3771), null, "user@example.com", true, "User", new DateTime(2025, 3, 29, 14, 39, 30, 650, DateTimeKind.Utc).AddTicks(3817), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "018e1606-956b-46d0-b86c-327e4fa48412", false, null, "user" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("eb69b15c-fcf8-44a0-98e3-9630a130052e"), new Guid("77a77e5c-3bcf-42bc-bc11-b6c604d55353") },
                    { new Guid("f9a3d333-ef36-4f94-b79f-aa040b538343"), new Guid("a3bef7de-3bc1-4682-8f25-23232fc4b21b") }
                });
        }
    }
}
