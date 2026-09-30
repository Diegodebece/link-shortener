using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkShortener.Infrastructure.Persistence.Config
{
    public class LinkClickConfiguration : IEntityTypeConfiguration<LinkClick>
    {
        public void Configure(EntityTypeBuilder<LinkClick> builder)
        {
            builder.ToTable("link_clicks");

            builder.HasKey(click => click.Id);

            builder.Property(click => click.Id)
                .ValueGeneratedOnAdd();

            builder.Property(click => click.ShortenedLinkId)
                .IsRequired()
                .HasColumnName("shortened_link_id");

            builder.Property(click => click.ClickedAt)
                .IsRequired()
                .HasColumnName("clicked_at");

            builder.Property(click => click.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");

            builder.Property(click => click.UserAgent)
                .HasMaxLength(512)
                .HasColumnName("user_agent");

            builder.Property(click => click.Referrer)
                .HasMaxLength(2048)
                .HasColumnName("referrer");

            builder.HasIndex(click => click.ShortenedLinkId);

            builder.HasIndex(click => click.ClickedAt);
        }
    }
}
