using Platform.Domain.DTOs.App;

namespace Platform.Application.Core.App.Queries.Handlers
{
    public interface ILoanQueryHandler
    {
        Task<IEnumerable<LoanDto>> GetAllLoans(CancellationToken cancellationToken);
        Task<LoanDto?> GetLoanById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<LoanDto>> GetLoansByUser(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<LoanDto>> GetFilteredLoans(LoanFilterDto filterDto, CancellationToken cancellationToken);
    }
}
