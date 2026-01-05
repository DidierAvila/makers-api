using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Loans
{
    public class GetFilteredLoans
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public GetFilteredLoans(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanDto>> HandleAsync(LoanFilterDto filterDto, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetFilteredLoansAsync(
                filterDto.UserId,
                filterDto.Status,
                filterDto.MinAmount,
                filterDto.MaxAmount,
                filterDto.StartDate,
                filterDto.EndDate,
                cancellationToken);

            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }
    }
}
