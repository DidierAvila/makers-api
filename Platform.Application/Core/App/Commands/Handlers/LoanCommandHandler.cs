using AutoMapper;
using Platform.Application.Core.App.Commands.Loans;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Handlers
{
    public class LoanCommandHandler : ILoanCommandHandler
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public LoanCommandHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<LoanDto> CreateLoan(Guid userId, CreateLoanDto createLoanDto, CancellationToken cancellationToken)
        {
            var createLoanCommand = new CreateLoan(_loanRepository, _mapper);
            return await createLoanCommand.HandleAsync(userId, createLoanDto, cancellationToken);
        }

        public async Task<LoanDto> UpdateLoanStatus(Guid loanId, Guid reviewerId, UpdateLoanStatusDto updateStatusDto, CancellationToken cancellationToken)
        {
            var updateLoanStatusCommand = new UpdateLoanStatus(_loanRepository, _mapper);
            return await updateLoanStatusCommand.HandleAsync(loanId, reviewerId, updateStatusDto, cancellationToken);
        }
    }
}
