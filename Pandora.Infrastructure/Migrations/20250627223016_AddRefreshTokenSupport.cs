using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReplacedByTokenId = table.Column<Guid>(type: "uuid", nullable: true),
                    RevocationReason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "NormalizedName", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("001ccad5-a952-4228-baa0-c117f081271e"), new DateTime(2025, 6, 27, 22, 30, 16, 61, DateTimeKind.Utc).AddTicks(4353), null, "User", "USER", null },
                    { new Guid("a1fdfc88-6f1d-4128-b82b-1e2e9d071525"), new DateTime(2025, 6, 27, 22, 30, 16, 61, DateTimeKind.Utc).AddTicks(4348), null, "Admin", "ADMIN", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "LastLoginDate", "LastName", "LastPasswordChangeDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUsername", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { new Guid("af8ee61f-1ba3-42ad-93c9-2aeb2ad490e5"), new DateTime(2025, 6, 27, 22, 30, 16, 61, DateTimeKind.Utc).AddTicks(8144), null, "user@example.com", true, "User", new DateTime(2025, 6, 27, 22, 30, 16, 61, DateTimeKind.Utc).AddTicks(8191), "User", null, false, null, "USER@EXAMPLE.COM", "USER", "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3", "1234567890", false, "df958d44-0c60-42f4-aedd-83d1c7de4c74", false, null, "user" },
                    { new Guid("dcd3e9de-a90c-424a-a8fd-80729bd101e0"), new DateTime(2025, 6, 27, 22, 30, 16, 61, DateTimeKind.Utc).AddTicks(7763), null, "admin@example.com", true, "Admin", new DateTime(2025, 6, 27, 22, 30, 16, 61, DateTimeKind.Utc).AddTicks(8141), "Admin", null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5", "1234567890", false, "246dd9b7-f921-4e80-bbfe-109344249cfd", false, null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("001ccad5-a952-4228-baa0-c117f081271e"), new Guid("af8ee61f-1ba3-42ad-93c9-2aeb2ad490e5") },
                    { new Guid("a1fdfc88-6f1d-4128-b82b-1e2e9d071525"), new Guid("dcd3e9de-a90c-424a-a8fd-80729bd101e0") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ExpiresAt",
                table: "RefreshTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked_IsUsed",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsRevoked", "IsUsed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("001ccad5-a952-4228-baa0-c117f081271e"), new Guid("af8ee61f-1ba3-42ad-93c9-2aeb2ad490e5") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1fdfc88-6f1d-4128-b82b-1e2e9d071525"), new Guid("dcd3e9de-a90c-424a-a8fd-80729bd101e0") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("001ccad5-a952-4228-baa0-c117f081271e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1fdfc88-6f1d-4128-b82b-1e2e9d071525"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("af8ee61f-1ba3-42ad-93c9-2aeb2ad490e5"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dcd3e9de-a90c-424a-a8fd-80729bd101e0"));

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
    }
}
