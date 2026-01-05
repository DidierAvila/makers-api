using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Platform.Domain.Entities.App;
using Platform.Domain.Repositories.App;
using Platform.Infrastructure.DbContexts;

namespace Platform.Infrastructure.Repositories.App
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(PlatformDbContext context, ILogger<RepositoryBase<Loan>> logger) : base(context, logger) { }

        public async Task<IEnumerable<Loan>> GetLoansByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new Loan
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Amount = l.Amount,
                    Term = l.Term,
                    Status = l.Status,
                    RequestedAt = l.RequestedAt,
                    ReviewedAt = l.ReviewedAt,
                    ReviewedBy = l.ReviewedBy,
                    ReviewNotes = l.ReviewNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    User = _context.Users.FirstOrDefault(u => u.Id == l.UserId),
                    Reviewer = l.ReviewedBy.HasValue ? _context.Users.FirstOrDefault(u => u.Id == l.ReviewedBy) : null
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetAllLoansWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new Loan
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Amount = l.Amount,
                    Term = l.Term,
                    Status = l.Status,
                    RequestedAt = l.RequestedAt,
                    ReviewedAt = l.ReviewedAt,
                    ReviewedBy = l.ReviewedBy,
                    ReviewNotes = l.ReviewNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    User = _context.Users.FirstOrDefault(u => u.Id == l.UserId),
                    Reviewer = l.ReviewedBy.HasValue ? _context.Users.FirstOrDefault(u => u.Id == l.ReviewedBy) : null
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .Where(l => l.Id == id)
                .Select(l => new Loan
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Amount = l.Amount,
                    Term = l.Term,
                    Status = l.Status,
                    RequestedAt = l.RequestedAt,
                    ReviewedAt = l.ReviewedAt,
                    ReviewedBy = l.ReviewedBy,
                    ReviewNotes = l.ReviewNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    User = _context.Users.FirstOrDefault(u => u.Id == l.UserId),
                    Reviewer = l.ReviewedBy.HasValue ? _context.Users.FirstOrDefault(u => u.Id == l.ReviewedBy) : null
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetLoansByStatusAsync(string status, CancellationToken cancellationToken = default)
        {
            return await _context.Loans
                .Where(l => l.Status == status)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new Loan
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Amount = l.Amount,
                    Term = l.Term,
                    Status = l.Status,
                    RequestedAt = l.RequestedAt,
                    ReviewedAt = l.ReviewedAt,
                    ReviewedBy = l.ReviewedBy,
                    ReviewNotes = l.ReviewNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    User = _context.Users.FirstOrDefault(u => u.Id == l.UserId),
                    Reviewer = l.ReviewedBy.HasValue ? _context.Users.FirstOrDefault(u => u.Id == l.ReviewedBy) : null
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Loan>> GetFilteredLoansAsync(
            Guid? userId,
            string? status,
            decimal? minAmount,
            decimal? maxAmount,
            DateTime? startDate,
            DateTime? endDate,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Loans.AsQueryable();

            if (userId.HasValue)
                query = query.Where(l => l.UserId == userId.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(l => l.Status == status);

            if (minAmount.HasValue)
                query = query.Where(l => l.Amount >= minAmount.Value);

            if (maxAmount.HasValue)
                query = query.Where(l => l.Amount <= maxAmount.Value);

            if (startDate.HasValue)
                query = query.Where(l => l.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(l => l.CreatedAt <= endDate.Value);

            return await query
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new Loan
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Amount = l.Amount,
                    Term = l.Term,
                    Status = l.Status,
                    RequestedAt = l.RequestedAt,
                    ReviewedAt = l.ReviewedAt,
                    ReviewedBy = l.ReviewedBy,
                    ReviewNotes = l.ReviewNotes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    User = _context.Users.FirstOrDefault(u => u.Id == l.UserId),
                    Reviewer = l.ReviewedBy.HasValue ? _context.Users.FirstOrDefault(u => u.Id == l.ReviewedBy) : null
                })
                .ToListAsync(cancellationToken);
        }
    }
}
