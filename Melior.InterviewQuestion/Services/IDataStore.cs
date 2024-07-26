namespace Melior.InterviewQuestion.Services
{
    public interface IDataStore
    {
        IAccount GetAccount(string accountNumber);
        void UpdateAccount(IAccount account);
    }
}
