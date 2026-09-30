using LinkShortener.Application.Common.Interfaces;

namespace LinkShortener.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LinkShortenerDbContext _dbContext;

        public UnitOfWork(LinkShortenerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
