using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Entities.App;
using Platform.Domain.Enums;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Loans
{
    public class CreateLoan
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public CreateLoan(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<LoanDto> HandleAsync(Guid userId, CreateLoanDto createLoanDto, CancellationToken cancellationToken)
        {
            // Validar que el monto y plazo sean válidos
            if (createLoanDto.Amount <= 0)
            {
                throw new InvalidOperationException("El monto del préstamo debe ser mayor a cero");
            }

            if (createLoanDto.Term <= 0)
            {
                throw new InvalidOperationException("El plazo del préstamo debe ser mayor a cero");
            }

            // Crear el préstamo
            var loan = _mapper.Map<Loan>(createLoanDto);
            loan.Id = Guid.NewGuid();
            loan.UserId = userId;
            loan.Status = LoanStatus.Pending;
            loan.RequestedAt = DateTime.UtcNow;
            loan.CreatedAt = DateTime.UtcNow;
            loan.UpdatedAt = DateTime.UtcNow;

            await _loanRepository.Create(loan, cancellationToken);

            // Obtener el préstamo con sus detalles
            var loanWithDetails = await _loanRepository.GetByIdWithDetailsAsync(loan.Id, cancellationToken);

            return _mapper.Map<LoanDto>(loanWithDetails);
        }
    }
}
