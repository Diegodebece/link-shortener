using LinkShortener.Domain.Entities;
using LinkShortener.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortener.Infrastructure.Persistence.Config
{
    internal class ShortenedLinkConfiguration : IEntityTypeConfiguration<ShortenedLink>
    {
        public void Configure(EntityTypeBuilder<ShortenedLink> builder)
        {
            builder.ToTable("shortened_links");
            builder.HasKey(link => link.Id);
            builder.Property(link => link.Id)
                .ValueGeneratedOnAdd();

            builder.Property(link => link.OriginalUrl)
                .HasConversion(
                originalUrl => originalUrl.Value,
                value => OriginalUrl.Create(value))
                .HasMaxLength(OriginalUrl.MaxLength)
                .IsRequired()
                .HasColumnName("original_url");

            builder.Property(link => link.ShortCode)
            .HasConversion(
                shortCode => shortCode.Value,
                value => ShortCode.Create(value))
            .HasMaxLength(ShortCode.MaxLength)
            .IsRequired()
            .HasColumnName("short_code");

            builder.HasIndex(link => link.ShortCode)
                .IsUnique();

            builder.Property(link => link.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at");

            builder.Property(link => link.ExpiresAt)
                .HasColumnName("expires_at");

            builder.Property(link => link.IsActive)
                .IsRequired()
                .HasColumnName("is_active");

            builder.Property(link => link.ClickCount)
                .IsRequired()
                .HasColumnName("click_count");

            builder.HasMany(link => link.Clicks)
                .WithOne()
                .HasForeignKey(click => click.ShortenedLinkId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
