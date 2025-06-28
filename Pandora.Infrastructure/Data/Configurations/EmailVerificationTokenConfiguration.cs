using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

namespace Pandora.Infrastructure.Data.Configurations;

public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        // Primary Key
        builder.HasKey(evt => evt.Id);
        
        // Properties
        builder.Property(evt => evt.Id)
               .IsRequired()
               .ValueGeneratedNever();

        builder.Property(evt => evt.UserId)
               .IsRequired();

        builder.Property(evt => evt.Token)
               .IsRequired()
               .HasMaxLength(256)
               .HasComment("Cryptographically secure verification token");

        builder.Property(evt => evt.Email)
               .IsRequired()
               .HasMaxLength(254)
               .HasComment("Email address to be verified");

        builder.Property(evt => evt.ExpiresAt)
               .IsRequired()
               .HasComment("When this token expires");

        builder.Property(evt => evt.IsUsed)
               .IsRequired()
               .HasDefaultValue(false)
               .HasComment("Whether this token has been used");

        builder.Property(evt => evt.IpAddress)
               .HasMaxLength(45) // Supports IPv6
               .HasComment("IP address when token was created");

        builder.Property(evt => evt.UserAgent)
               .HasMaxLength(500)
               .HasComment("User agent when token was requested");

        // Relationships
        builder.HasOne(evt => evt.User)
               .WithMany(u => u.EmailVerificationTokens)
               .HasForeignKey(evt => evt.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance and security
        builder.HasIndex(evt => evt.Token)
               .IsUnique()
               .HasDatabaseName("IX_EmailVerificationTokens_Token");

        builder.HasIndex(evt => evt.UserId)
               .HasDatabaseName("IX_EmailVerificationTokens_UserId");

        builder.HasIndex(evt => evt.Email)
               .HasDatabaseName("IX_EmailVerificationTokens_Email");

        builder.HasIndex(evt => evt.ExpiresAt)
               .HasDatabaseName("IX_EmailVerificationTokens_ExpiresAt");

        builder.HasIndex(evt => new { evt.IpAddress, evt.CreatedDate })
               .HasDatabaseName("IX_EmailVerificationTokens_IpAddress_CreatedDate");

        // Table name
        builder.ToTable("EmailVerificationTokens");
    }
} 