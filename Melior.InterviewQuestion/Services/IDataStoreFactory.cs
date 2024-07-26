namespace Melior.InterviewQuestion.Services
{
    public interface IDataStoreFactory
    {
        IDataStore CreateDataStore(string type);
    }
}