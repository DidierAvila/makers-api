using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.DTOs.App
{
    public class CreateLoanDto
    {
        [Required(ErrorMessage = "El monto es requerido")]
        [Range(1, 1000000000, ErrorMessage = "El monto debe estar entre 1 y 1,000,000,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "El plazo es requerido")]
        [Range(1, 360, ErrorMessage = "El plazo debe estar entre 1 y 360 meses")]
        public int Term { get; set; }
    }
}
