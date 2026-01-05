namespace Platform.Domain.DTOs.App
{
    public class LoanDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public Guid? ReviewedBy { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
