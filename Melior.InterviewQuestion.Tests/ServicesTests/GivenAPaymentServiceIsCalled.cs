using FluentAssertions;
using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
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
            mockIDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(new Account());
            mockIDataStoreFactory = new Mock<IDataStoreFactory>();
            mockIDataStoreFactory.Setup(x => x.CreateDataStore(It.IsAny<string>())).Returns(Mock.Of<IDataStore>());


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

        [Fact]
        public void MakePayment_WhenInvokesWithRequest_ThenNoExcpetionIsThrown()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 400,
                CreditorAccountNumber = "Test001",
                DebtorAccountNumber = "Test002",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments,
            };

            var act = () => systemUnderTest.MakePayment(request);
            act.Should().NotThrow();
        }
    }
}
