using FluentAssertions;
using Melior.InterviewQuestion.Services;
using Moq;
using System;
using Xunit;

namespace Melior.InterviewQuestion.Tests.ServicesTests
{
    public class GivenAPaymentServiceIsCalled
    {
        private readonly Mock<IDataStore> mockIDataStore;
        private readonly Mock<IDataStoreFactory> mockIDataStoreFactory;
        private readonly PaymentService systemUnderTest;

        public GivenAPaymentServiceIsCalled()
        {
            mockIDataStore = new Mock<IDataStore>();
            mockIDataStoreFactory = new Mock<IDataStoreFactory>();


            systemUnderTest = new PaymentService(mockIDataStore.Object, mockIDataStoreFactory.Object);
        }

        [Fact]
        public void Constructor_WhenNullIDataStoreIsSupplied_ThenTheExpectedErrorIsThrown()
        {
            var act = () => new PaymentService(null, mockIDataStoreFactory.Object);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("dataStore"); ;
        }      

        [Fact]
        public void Constructor_WhenNullIDataStoreFactoryIsSupplied_ThenTheExpectedErrorIsThrown()
        {
            var act = () => new PaymentService(mockIDataStore.Object, null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("dataStoreFactory"); ;
        }   
    }
}
