using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pandora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PersonalVaultPropertyNamesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PersonalVaults",
                newName: "SecureUrl");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "PersonalVaults",
                newName: "SecureTags");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "PersonalVaults",
                newName: "SecureTitle");

            migrationBuilder.RenameColumn(
                name: "EncryptedUrl",
                table: "PersonalVaults",
                newName: "SecureSummary");

            migrationBuilder.RenameColumn(
                name: "EncryptedMediaFile",
                table: "PersonalVaults",
                newName: "SecureMediaFile");

            migrationBuilder.RenameColumn(
                name: "EncryptedContent",
                table: "PersonalVaults",
                newName: "SecureContent");

            migrationBuilder.RenameColumn(
                name: "SiteName",
                table: "PasswordVaults",
                newName: "SecureSiteName");

            migrationBuilder.RenameColumn(
                name: "EncryptedUsernameOrEmail",
                table: "PasswordVaults",
                newName: "SecureUsernameOrEmail");

            migrationBuilder.RenameColumn(
                name: "EncryptedNotes",
                table: "PasswordVaults",
                newName: "SecureNotes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecureUrl",
                table: "PersonalVaults",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "SecureTitle",
                table: "PersonalVaults",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "SecureTags",
                table: "PersonalVaults",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "SecureSummary",
                table: "PersonalVaults",
                newName: "EncryptedUrl");

            migrationBuilder.RenameColumn(
                name: "SecureMediaFile",
                table: "PersonalVaults",
                newName: "EncryptedMediaFile");

            migrationBuilder.RenameColumn(
                name: "SecureContent",
                table: "PersonalVaults",
                newName: "EncryptedContent");

            migrationBuilder.RenameColumn(
                name: "SecureUsernameOrEmail",
                table: "PasswordVaults",
                newName: "EncryptedUsernameOrEmail");

            migrationBuilder.RenameColumn(
                name: "SecureSiteName",
                table: "PasswordVaults",
                newName: "SiteName");

            migrationBuilder.RenameColumn(
                name: "SecureNotes",
                table: "PasswordVaults",
                newName: "EncryptedNotes");
        }
    }
}
