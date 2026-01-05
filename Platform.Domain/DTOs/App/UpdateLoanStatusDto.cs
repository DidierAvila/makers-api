using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.DTOs.App
{
    public class UpdateLoanStatusDto
    {
        [Required(ErrorMessage = "El estado es requerido")]
        public required string Status { get; set; }

        [MaxLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres")]
        public string? ReviewNotes { get; set; }
    }
}
