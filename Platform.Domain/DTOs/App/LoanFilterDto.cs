namespace Platform.Domain.DTOs.App
{
    public class LoanFilterDto
    {
        public Guid? UserId { get; set; }
        public string? Status { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
