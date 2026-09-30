using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortener.Infrastructure.Persistence
{
    public class LinkShortenerDbContext : DbContext
    {
        public LinkShortenerDbContext(DbContextOptions<LinkShortenerDbContext> options) : base(options)
        {
        }

        public DbSet<ShortenedLink> ShortenedLinks => Set<ShortenedLink>();

        public DbSet<LinkClick> LinkClicks => Set<LinkClick>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkShortenerDbContext).Assembly);
        }
    }

}
