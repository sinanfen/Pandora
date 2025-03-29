using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingTheDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a098e93d-cfaf-45a7-a47e-57c10dd92afd"), new Guid("49a58a56-bcbd-45fc-a8f8-dec748059765") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("07250819-3df0-4bc3-acf1-0ee372d43bbe"), new Guid("6854a44d-631e-439b-8f8e-09e8e4576a95") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("07250819-3df0-4bc3-acf1-0ee372d43bbe"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a098e93d-cfaf-45a7-a47e-57c10dd92afd"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("49a58a56-bcbd-45fc-a8f8-dec748059765"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6854a44d-631e-439b-8f8e-09e8e4576a95"));

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "PersonalVaults",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockDate",
                table: "PersonalVaults",
                type: "timestamp with time zone",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "PersonalVaults");

            migrationBuilder.DropColumn(
                name: "UnlockDate",
                table: "PersonalVaults");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("07250819-3df0-4bc3-acf1-0ee372d43bbe"), new DateTime(2024, 10, 20, 0, 46, 41, 36, DateTimeKind.Utc).AddTicks(9348), null, "Admin", "ADMIN", null },
                    { new Guid("a098e93d-cfaf-45a7-a47e-57c10dd92afd"), new DateTime(2024, 10, 20, 0, 46, 41, 36, DateTimeKind.Utc).AddTicks(9354), null, "User", "USER", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("49a58a56-bcbd-45fc-a8f8-dec748059765"), new DateTime(2024, 10, 20, 0, 46, 41, 37, DateTimeKind.Utc).AddTicks(3117), null, "user@example.com", true, "User", new DateTime(2024, 10, 20, 0, 46, 41, 37, DateTimeKind.Utc).AddTicks(3182), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "2af9f001-08b2-4198-8bc5-b884b1cd4507", false, null, "user" },
                    { new Guid("6854a44d-631e-439b-8f8e-09e8e4576a95"), new DateTime(2024, 10, 20, 0, 46, 41, 37, DateTimeKind.Utc).AddTicks(2698), null, "admin@example.com", true, "Admin", new DateTime(2024, 10, 20, 0, 46, 41, 37, DateTimeKind.Utc).AddTicks(3114), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "8a9ad6bd-54d0-4c6c-9a04-191daa925c12", false, null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("a098e93d-cfaf-45a7-a47e-57c10dd92afd"), new Guid("49a58a56-bcbd-45fc-a8f8-dec748059765") },
                    { new Guid("07250819-3df0-4bc3-acf1-0ee372d43bbe"), new Guid("6854a44d-631e-439b-8f8e-09e8e4576a95") }
                });
        }
    }
}
