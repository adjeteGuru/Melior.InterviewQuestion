using Melior.InterviewQuestion.Data;
using Xunit;

namespace Melior.InterviewQuestion.Tests
{
    public class GivenDataStoreFactoryIsCalled
    {
        private readonly DataStoreFactory systemUnderTest;

        public GivenDataStoreFactoryIsCalled()
        {
            systemUnderTest = new DataStoreFactory();
        }

        [Fact]
        public void Test1()
        {

        }
    }
}