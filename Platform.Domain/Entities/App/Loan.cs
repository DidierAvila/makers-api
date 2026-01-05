using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Platform.Domain.Entities.Auth;

namespace Platform.Domain.Entities.App
{
    [Table("Loans", Schema = "App")]
    public partial class Loan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Amount { get; set; }

        [Required]
        public required int Term { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Status { get; set; }

        public DateTime? RequestedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public Guid? ReviewedBy { get; set; }

        [MaxLength(500)]
        public string? ReviewNotes { get; set; }

        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public virtual User? User { get; set; }

        public virtual User? Reviewer { get; set; }
    }
}
