using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Enums;
using Platform.Domain.Repositories.App;

namespace Platform.Application.Core.App.Commands.Loans
{
    public class UpdateLoanStatus
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public UpdateLoanStatus(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<LoanDto> HandleAsync(Guid loanId, Guid reviewerId, UpdateLoanStatusDto updateStatusDto, CancellationToken cancellationToken)
        {
            // Validar que el estado sea válido
            if (!LoanStatus.IsValid(updateStatusDto.Status))
            {
                throw new InvalidOperationException($"Estado de préstamo inválido: {updateStatusDto.Status}");
            }

            // Obtener el préstamo
            var loan = await _loanRepository.GetByID(loanId, cancellationToken);

            if (loan == null)
            {
                throw new KeyNotFoundException($"No se encontró el préstamo con ID: {loanId}");
            }

            // Validar que el préstamo esté pendiente
            if (loan.Status != LoanStatus.Pending)
            {
                throw new InvalidOperationException($"El préstamo ya fue {loan.Status.ToLower()}");
            }

            // Actualizar el estado del préstamo
            loan.Status = updateStatusDto.Status;
            loan.ReviewedBy = reviewerId;
            loan.ReviewedAt = DateTime.UtcNow;
            loan.ReviewNotes = updateStatusDto.ReviewNotes;
            loan.UpdatedAt = DateTime.UtcNow;

            await _loanRepository.Update(loan, cancellationToken);

            // Obtener el préstamo actualizado con sus detalles
            var loanWithDetails = await _loanRepository.GetByIdWithDetailsAsync(loanId, cancellationToken);

            return _mapper.Map<LoanDto>(loanWithDetails);
        }
    }
}
