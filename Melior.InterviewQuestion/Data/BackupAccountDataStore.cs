using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Data
{
    public class BackupAccountDataStore : IDataStore
    {
        public Account GetAccount(string accountNumber)
        {
            // Access backup data base to retrieve account, code removed for brevity 
            return new Account()
            {
                AccountNumber = accountNumber,
            };
        }

        public void UpdateAccount(Account account)
        {
            // Update account in backup database, code removed for brevity
        }
    }
}
