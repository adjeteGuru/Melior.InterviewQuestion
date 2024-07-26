using System.Configuration;

namespace Melior.InterviewQuestion.Services
{
    public interface IConfiguration
    {
        string DataStoreType { get; }
    }

    public class Configuration : IConfiguration
    {
        public string DataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
    }
}
