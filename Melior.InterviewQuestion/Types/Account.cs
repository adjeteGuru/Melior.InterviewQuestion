using Melior.InterviewQuestion.Services;

namespace Melior.InterviewQuestion.Types
{
    public class Account : IAccount
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }
    }
}

