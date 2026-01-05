using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Platform.Application.Core.App.Queries.Loans;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Handlers
{
    public class LoanQueryHandler : ILoanQueryHandler
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public LoanQueryHandler(ILoanRepository loanRepository, IMapper mapper, IMemoryCache cache)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<LoanDto>> GetAllLoans(CancellationToken cancellationToken)
        {
            var getAllLoansQuery = new GetAllLoans(_loanRepository, _mapper, _cache);
            return await getAllLoansQuery.HandleAsync(cancellationToken);
        }

        public async Task<LoanDto?> GetLoanById(Guid id, CancellationToken cancellationToken)
        {
            var getLoanByIdQuery = new GetLoanById(_loanRepository, _mapper, _cache);
            return await getLoanByIdQuery.HandleAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByUser(Guid userId, CancellationToken cancellationToken)
        {
            var getLoansByUserQuery = new GetLoansByUser(_loanRepository, _mapper, _cache);
            return await getLoansByUserQuery.HandleAsync(userId, cancellationToken);
        }

        public async Task<IEnumerable<LoanDto>> GetFilteredLoans(LoanFilterDto filterDto, CancellationToken cancellationToken)
        {
            var getFilteredLoansQuery = new GetFilteredLoans(_loanRepository, _mapper);
            return await getFilteredLoansQuery.HandleAsync(filterDto, cancellationToken);
        }
    }
}
