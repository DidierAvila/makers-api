using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Loans
{
    public class GetAllLoans
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "AllLoans";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public GetAllLoans(ILoanRepository loanRepository, IMapper mapper, IMemoryCache cache)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<LoanDto>> HandleAsync(CancellationToken cancellationToken)
        {
            // Intentar obtener del caché
            if (_cache.TryGetValue(CacheKey, out IEnumerable<LoanDto>? cachedLoans) && cachedLoans != null)
            {
                return cachedLoans;
            }

            // Si no está en caché, obtener de la base de datos
            var loans = await _loanRepository.GetAllLoansWithDetailsAsync(cancellationToken);
            var loanDtos = _mapper.Map<IEnumerable<LoanDto>>(loans);

            // Guardar en caché
            _cache.Set(CacheKey, loanDtos, _cacheExpiration);

            return loanDtos;
        }
    }
}
