using Platform.Domain.Entities.App;

namespace Platform.Domain.Repositories.App
{
    public interface ILoanRepository : IRepositoryBase<Loan>
    {
        Task<IEnumerable<Loan>> GetLoansByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetAllLoansWithDetailsAsync(CancellationToken cancellationToken = default);
        Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetLoansByStatusAsync(string status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Loan>> GetFilteredLoansAsync(Guid? userId, string? status, decimal? minAmount, decimal? maxAmount, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    }
}
