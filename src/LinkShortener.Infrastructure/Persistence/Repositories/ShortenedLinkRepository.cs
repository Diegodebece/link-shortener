using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Repositories;
using LinkShortener.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortener.Infrastructure.Persistence.Repositories
{
    public class ShortenedLinkRepository : IShortenedLinkRepository
    {
        private readonly LinkShortenerDbContext _dbContext;

        public ShortenedLinkRepository(LinkShortenerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsByShortCodeAsync(
           ShortCode shortCode,
           CancellationToken cancellationToken = default)
        {
            return _dbContext.ShortenedLinks
                .AnyAsync(link => link.ShortCode == shortCode, cancellationToken);
        }

        public Task<ShortenedLink?> GetByShortCodeAsync(
            ShortCode shortCode,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.ShortenedLinks
                .Include(link => link.Clicks)
                .FirstOrDefaultAsync(link => link.ShortCode == shortCode, cancellationToken);
        }

        public async Task AddAsync(
            ShortenedLink shortenedLink,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.ShortenedLinks.AddAsync(shortenedLink, cancellationToken);
        }

    }
}
