using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
using System;

namespace Melior.InterviewQuestion.Data
{
    public class AccountDataStore : IDataStore
    {
        public IAccount GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
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
            if (account == null)
            {
                throw new Exception("There is no account found to update.");
            }
            // Update account in database, code removed for brevity
        }
    }
}
