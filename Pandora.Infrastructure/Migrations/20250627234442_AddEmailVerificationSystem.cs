using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "EmailVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "Cryptographically secure verification token"),
                    Email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false, comment: "Email address to be verified"),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "When this token expires"),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Whether this token has been used"),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true, comment: "IP address when token was created"),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "User agent when token was requested"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerificationTokens_Users_UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_Email",
                table: "EmailVerificationTokens",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_ExpiresAt",
                table: "EmailVerificationTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_IpAddress_CreatedDate",
                table: "EmailVerificationTokens",
                columns: new[] { "IpAddress", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_Token",
                table: "EmailVerificationTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_UserId",
                table: "EmailVerificationTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerificationTokens");

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
        }
    }
}
