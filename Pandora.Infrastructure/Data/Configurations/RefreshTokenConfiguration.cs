using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandora.Core.Domain.Entities;

namespace Pandora.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Primary key
        builder.HasKey(rt => rt.Id);

        // Token should be unique and indexed for fast lookups
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasIndex(rt => rt.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshTokens_Token");

        // User relationship
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Properties
        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(rt => rt.IpAddress)
            .HasMaxLength(45); // IPv6 max length

        builder.Property(rt => rt.UserAgent)
            .HasMaxLength(500);

        builder.Property(rt => rt.RevocationReason)
            .HasMaxLength(200);

        // Indexes for performance
        builder.HasIndex(rt => rt.UserId)
            .HasDatabaseName("IX_RefreshTokens_UserId");

        builder.HasIndex(rt => rt.ExpiresAt)
            .HasDatabaseName("IX_RefreshTokens_ExpiresAt");

        builder.HasIndex(rt => new { rt.UserId, rt.IsRevoked, rt.IsUsed })
            .HasDatabaseName("IX_RefreshTokens_UserId_IsRevoked_IsUsed");

        // Table name
        builder.ToTable("RefreshTokens");
    }
} 