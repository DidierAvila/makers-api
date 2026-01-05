using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Application.Core.App.Commands.Handlers;
using Platform.Application.Core.App.Queries.Handlers;
using Platform.Application.Utils;
using Platform.Domain.DTOs.App;
using System.Security.Claims;

namespace Platform.Api.Controllers.App
{
    /// <summary>
    /// Controlador para la gestión de préstamos bancarios.
    /// </summary>
    [ApiController]
    [Route("Api/[controller]")]
    [Authorize]
    public class LoansController : ControllerBase
    {
        private readonly ILogger<LoansController> _logger;
        private readonly ILoanCommandHandler _loanCommandHandler;
        private readonly ILoanQueryHandler _loanQueryHandler;

        public LoansController(
            ILogger<LoansController> logger,
            ILoanCommandHandler loanCommandHandler,
            ILoanQueryHandler loanQueryHandler)
        {
            _logger = logger;
            _loanCommandHandler = loanCommandHandler;
            _loanQueryHandler = loanQueryHandler;
        }

        /// <summary>
        /// Crea una nueva solicitud de préstamo.
        /// </summary>
        /// <param name="createLoanDto">Datos del préstamo a crear</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>El préstamo creado</returns>
        /// <response code="201">Préstamo creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="401">No autenticado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(LoanDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto createLoanDto, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("CreateLoan endpoint called");

                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(CustomClaimTypes.UserId)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    _logger.LogWarning("Invalid or missing user ID in token");
                    return Unauthorized(new { message = "Invalid or missing user ID in token" });
                }

                var loan = await _loanCommandHandler.CreateLoan(userId, createLoanDto, cancellationToken);

                _logger.LogInformation("Loan created successfully with ID: {LoanId}", loan.Id);
                return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation when creating loan");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating loan");
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene todos los préstamos (solo administradores).
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de todos los préstamos</returns>
        /// <response code="200">Lista de préstamos</response>
        /// <response code="401">No autenticado</response>
        /// <response code="403">No autorizado (solo administradores)</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<LoanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLoans(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetAllLoans endpoint called");

                var loans = await _loanQueryHandler.GetAllLoans(cancellationToken);

                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all loans");
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un préstamo por su ID.
        /// </summary>
        /// <param name="id">ID del préstamo</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>El préstamo solicitado</returns>
        /// <response code="200">Préstamo encontrado</response>
        /// <response code="401">No autenticado</response>
        /// <response code="404">Préstamo no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoanById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetLoanById endpoint called for ID: {LoanId}", id);

                var loan = await _loanQueryHandler.GetLoanById(id, cancellationToken);

                if (loan == null)
                {
                    return NotFound(new { success = false, message = $"Loan with ID {id} not found" });
                }

                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loan by ID: {LoanId}", id);
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los préstamos del usuario autenticado.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de préstamos del usuario</returns>
        /// <response code="200">Lista de préstamos del usuario</response>
        /// <response code="401">No autenticado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("my-loans")]
        [ProducesResponseType(typeof(IEnumerable<LoanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyLoans(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetMyLoans endpoint called");

                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(CustomClaimTypes.UserId)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    _logger.LogWarning("Invalid or missing user ID in token");
                    return Unauthorized(new { message = "Invalid or missing user ID in token" });
                }

                var loans = await _loanQueryHandler.GetLoansByUser(userId, cancellationToken);

                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user loans");
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Aprueba un préstamo (solo administradores).
        /// </summary>
        /// <param name="id">ID del préstamo</param>
        /// <param name="updateStatusDto">Datos de la revisión</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>El préstamo actualizado</returns>
        /// <response code="200">Préstamo aprobado exitosamente</response>
        /// <response code="400">Datos inválidos o préstamo ya revisado</response>
        /// <response code="401">No autenticado</response>
        /// <response code="403">No autorizado (solo administradores)</response>
        /// <response code="404">Préstamo no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLoanStatus(Guid id, [FromBody] UpdateLoanStatusDto updateStatusDto, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("UpdateLoanStatus endpoint called for loan ID: {LoanId}", id);

                // Obtener el ID del usuario administrador desde el token JWT
                var reviewerIdClaim = User.FindFirst(CustomClaimTypes.UserId)?.Value;

                if (string.IsNullOrEmpty(reviewerIdClaim) || !Guid.TryParse(reviewerIdClaim, out var reviewerId))
                {
                    _logger.LogWarning("Invalid or missing reviewer ID in token");
                    return Unauthorized(new { message = "Invalid or missing reviewer ID in token" });
                }

                var loan = await _loanCommandHandler.UpdateLoanStatus(id, reviewerId, updateStatusDto, cancellationToken);

                _logger.LogInformation("Loan status updated successfully for ID: {LoanId}", id);
                return Ok(loan);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Loan not found with ID: {LoanId}", id);
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation when updating loan status");
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating loan status");
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene préstamos filtrados (solo administradores).
        /// </summary>
        /// <param name="userId">Filtrar por usuario</param>
        /// <param name="status">Filtrar por estado</param>
        /// <param name="minAmount">Monto mínimo</param>
        /// <param name="maxAmount">Monto máximo</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Lista de préstamos filtrados</returns>
        /// <response code="200">Lista de préstamos filtrados</response>
        /// <response code="401">No autenticado</response>
        /// <response code="403">No autorizado (solo administradores)</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("filtered")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<LoanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilteredLoans(
            [FromQuery] Guid? userId,
            [FromQuery] string? status,
            [FromQuery] decimal? minAmount,
            [FromQuery] decimal? maxAmount,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetFilteredLoans endpoint called");

                var filterDto = new LoanFilterDto
                {
                    UserId = userId,
                    Status = status,
                    MinAmount = minAmount,
                    MaxAmount = maxAmount,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var loans = await _loanQueryHandler.GetFilteredLoans(filterDto, cancellationToken);

                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting filtered loans");
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }
    }
}
