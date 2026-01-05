using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Platform.Domain.DTOs.App;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Queries.Loans
{
    public class GetLoansByUser
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public GetLoansByUser(ILoanRepository loanRepository, IMapper mapper, IMemoryCache cache)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<LoanDto>> HandleAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cacheKey = $"UserLoans_{userId}";

            // Intentar obtener del caché
            if (_cache.TryGetValue(cacheKey, out IEnumerable<LoanDto>? cachedLoans) && cachedLoans != null)
            {
                return cachedLoans;
            }

            // Si no está en caché, obtener de la base de datos
            var loans = await _loanRepository.GetLoansByUserIdAsync(userId, cancellationToken);
            var loanDtos = _mapper.Map<IEnumerable<LoanDto>>(loans);

            // Guardar en caché
            _cache.Set(cacheKey, loanDtos, _cacheExpiration);

            return loanDtos;
        }
    }
}
