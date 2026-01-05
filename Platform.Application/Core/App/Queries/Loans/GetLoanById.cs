using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Loans
{
    public class GetLoanById
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public GetLoanById(ILoanRepository loanRepository, IMapper mapper, IMemoryCache cache)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<LoanDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = $"Loan_{id}";

            // Intentar obtener del caché
            if (_cache.TryGetValue(cacheKey, out LoanDto? cachedLoan) && cachedLoan != null)
            {
                return cachedLoan;
            }

            // Si no está en caché, obtener de la base de datos
            var loan = await _loanRepository.GetByIdWithDetailsAsync(id, cancellationToken);

            if (loan == null)
                return null;

            var loanDto = _mapper.Map<LoanDto>(loan);

            // Guardar en caché
            _cache.Set(cacheKey, loanDto, _cacheExpiration);

            return loanDto;
        }
    }
}
