using Platform.Domain.DTOs.App;

namespace Platform.Application.Core.App.Commands.Handlers
{
    public interface ILoanCommandHandler
    {
        Task<LoanDto> CreateLoan(Guid userId, CreateLoanDto createLoanDto, CancellationToken cancellationToken);
        Task<LoanDto> UpdateLoanStatus(Guid loanId, Guid reviewerId, UpdateLoanStatusDto updateStatusDto, CancellationToken cancellationToken);
    }
}
