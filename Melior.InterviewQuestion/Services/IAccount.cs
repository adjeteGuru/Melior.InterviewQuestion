using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Services
{
    public interface IAccount
    {
        string AccountNumber { get; set; }
        AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }
        decimal Balance { get; set; }
        AccountStatus Status { get; set; }
    }
}