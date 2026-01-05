namespace Platform.Domain.Enums
{
    public static class LoanStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";

        public static readonly string[] All = { Pending, Approved, Rejected };

        public static bool IsValid(string status)
        {
            return All.Contains(status);
        }
    }
}
