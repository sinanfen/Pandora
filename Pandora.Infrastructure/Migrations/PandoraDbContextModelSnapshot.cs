﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pandora.Infrastructure.Data.Contexts;

#nullable disable

namespace Pandora.Infrastructure.Migrations
{
    [DbContext(typeof(PandoraDbContext))]
    partial class PandoraDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pandora.Core.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedDate");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.EmailVerificationToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasComment("Email address to be verified");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasComment("When this token expires");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)")
                        .HasComment("IP address when token was created");

                    b.Property<bool>("IsUsed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasComment("Whether this token has been used");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasComment("Cryptographically secure verification token");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasComment("User agent when token was requested");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .HasDatabaseName("IX_EmailVerificationTokens_Email");

                    b.HasIndex("ExpiresAt")
                        .HasDatabaseName("IX_EmailVerificationTokens_ExpiresAt");

                    b.HasIndex("Token")
                        .IsUnique()
                        .HasDatabaseName("IX_EmailVerificationTokens_Token");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IX_EmailVerificationTokens_UserId");

                    b.HasIndex("IpAddress", "CreatedDate")
                        .HasDatabaseName("IX_EmailVerificationTokens_IpAddress_CreatedDate");

                    b.ToTable("EmailVerificationTokens", (string)null);
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.PasswordVault", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedDate");

                    b.Property<DateTime?>("LastPasswordChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecureNotes")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("SecureSiteName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("SecureUsernameOrEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordVaults", (string)null);
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.PersonalVault", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedDate");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsShareable")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SecureContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecureMediaFile")
                        .HasColumnType("text");

                    b.Property<string>("SecureSummary")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("SecureTags")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("SecureTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecureUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShareToken")
                        .HasColumnType("text");

                    b.Property<int>("ShareViewCount")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("SharedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UnlockDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("PersonalVaults");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(45)
                        .HasColumnType("character varying(45)");

                    b.Property<bool>("IsRevoked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsUsed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("ReplacedByTokenId")
                        .HasColumnType("uuid");

                    b.Property<string>("RevocationReason")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ExpiresAt")
                        .HasDatabaseName("IX_RefreshTokens_ExpiresAt");

                    b.HasIndex("Token")
                        .IsUnique()
                        .HasDatabaseName("IX_RefreshTokens_Token");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IX_RefreshTokens_UserId");

                    b.HasIndex("UserId", "IsRevoked", "IsUsed")
                        .HasDatabaseName("IX_RefreshTokens_UserId_IsRevoked_IsUsed");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("dc7bbe32-07ac-4f2c-88ba-f81abddf2979"),
                            CreatedDate = new DateTime(2025, 6, 28, 18, 19, 56, 227, DateTimeKind.Utc).AddTicks(7545),
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("a97e48d0-0d5a-47a6-b032-7f84f282c42a"),
                            CreatedDate = new DateTime(2025, 6, 28, 18, 19, 56, 227, DateTimeKind.Utc).AddTicks(7558),
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DeletedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastPasswordChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUsername")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TwoFactorBackupCodes")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("TwoFactorEnabledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TwoFactorSecretKey")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .IsUnique();

                    b.HasIndex("NormalizedUsername")
                        .IsUnique();

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("0fb0e9db-77ab-490d-baf0-a5f7dbd76622"),
                            CreatedDate = new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(1510),
                            Email = "admin@example.com",
                            EmailConfirmed = true,
                            FirstName = "Admin",
                            LastLoginDate = new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(1978),
                            LastName = "Admin",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@EXAMPLE.COM",
                            NormalizedUsername = "ADMIN",
                            PasswordHash = "d48a3b22dbfe455ae3438f7d89eb62875ddfc2234d2acc6f32132385d049f65752a5858dbb19006ff4882134264607869639798e06ec48928997f72c4ed31be5",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "b188f86b-85f8-4fa7-9324-874459d68fb3",
                            TwoFactorEnabled = false,
                            Username = "admin"
                        },
                        new
                        {
                            Id = new Guid("ce80c84a-6885-45d5-9a45-2ebb129537c4"),
                            CreatedDate = new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(1981),
                            Email = "user@example.com",
                            EmailConfirmed = true,
                            FirstName = "User",
                            LastLoginDate = new DateTime(2025, 6, 28, 18, 19, 56, 228, DateTimeKind.Utc).AddTicks(2036),
                            LastName = "User",
                            LockoutEnabled = false,
                            NormalizedEmail = "USER@EXAMPLE.COM",
                            NormalizedUsername = "USER",
                            PasswordHash = "e86424a11261f6897e28276662338ebfd4b3230c1295647590ab8c510fd1557355b92fc9051296f7ac5c331a52fa5c2d22cf76c2fc61fcbd436a317e02bf53b3",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "806e5b53-4cb4-4502-a268-5d6124c1a7ea",
                            TwoFactorEnabled = false,
                            Username = "user"
                        });
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("0fb0e9db-77ab-490d-baf0-a5f7dbd76622"),
                            RoleId = new Guid("dc7bbe32-07ac-4f2c-88ba-f81abddf2979")
                        },
                        new
                        {
                            UserId = new Guid("ce80c84a-6885-45d5-9a45-2ebb129537c4"),
                            RoleId = new Guid("a97e48d0-0d5a-47a6-b032-7f84f282c42a")
                        });
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.Category", b =>
                {
                    b.HasOne("Pandora.Core.Domain.Entities.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.EmailVerificationToken", b =>
                {
                    b.HasOne("Pandora.Core.Domain.Entities.User", "User")
                        .WithMany("EmailVerificationTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.PasswordVault", b =>
                {
                    b.HasOne("Pandora.Core.Domain.Entities.Category", "Category")
                        .WithMany("PasswordVaults")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Pandora.Core.Domain.Entities.User", "User")
                        .WithMany("PasswordVaults")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.PersonalVault", b =>
                {
                    b.HasOne("Pandora.Core.Domain.Entities.Category", "Category")
                        .WithMany("PersonalVaults")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Pandora.Core.Domain.Entities.User", "User")
                        .WithMany("PersonalVaults")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("Pandora.Core.Domain.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("Pandora.Core.Domain.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pandora.Core.Domain.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.Category", b =>
                {
                    b.Navigation("PasswordVaults");

                    b.Navigation("PersonalVaults");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Pandora.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("EmailVerificationTokens");

                    b.Navigation("PasswordVaults");

                    b.Navigation("PersonalVaults");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
