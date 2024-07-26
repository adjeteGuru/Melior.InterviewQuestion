using Melior.InterviewQuestion.Services;

namespace Melior.InterviewQuestion.Data
{
    public class DataStoreFactory : IDataStoreFactory    
    {
        public IDataStore CreateDataStore(string type)
        {
            return type == "Backup" ? new BackupAccountDataStore() : new AccountDataStore();
        }
    }
}
