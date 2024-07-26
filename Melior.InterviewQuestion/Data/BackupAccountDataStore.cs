using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
using System;

namespace Melior.InterviewQuestion.Data
{
    public class BackupAccountDataStore : IDataStore
    {
        public IAccount GetAccount(string accountNumber)
        {
            // Access backup data base to retrieve account, code removed for brevity 
            if (!string.IsNullOrEmpty(accountNumber))
            {
                return new Account
                {
                    AccountNumber = accountNumber,
                };
            }
            throw new ArgumentException();
        }

        public void UpdateAccount(IAccount account)
        {
            // Update account in backup database, code removed for brevity
        }
    }
}
