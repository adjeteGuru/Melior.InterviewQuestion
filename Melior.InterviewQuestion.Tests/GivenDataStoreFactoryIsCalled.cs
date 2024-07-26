using FluentAssertions;
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
        public void CreateDataStore_WhenInvokesWithType_ThenNoExceptionIsThrown()
        {
            var act = () => systemUnderTest.CreateDataStore("test");
            act.Should().NotThrow();
        }

        [Fact]
        public void CreateDataStore_WhenInvokesWithBackupType_ThenTheExpectedResultTypeIsReturned()
        {
            var result = systemUnderTest.CreateDataStore("Backup");
            result.Should().BeOfType<BackupAccountDataStore>();
        }

        [Theory]
        [InlineData("type")]
        [InlineData("")]
        [InlineData(" ")]
        public void CreateDataStore_WhenInvokesWithOtherType_ThenTheExpectedResultTypeIsReturned(string type)
        {
            var result = systemUnderTest.CreateDataStore(type);
            result.Should().BeOfType<AccountDataStore>();
        }
    }
}