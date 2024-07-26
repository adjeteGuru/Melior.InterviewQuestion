using FluentAssertions;
using Melior.InterviewQuestion.Data;
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
        private readonly Mock<IConfiguration> mockIConfiguration;
        private readonly Mock<IAccount> mockIAccount;
        private readonly PaymentService systemUnderTest;

        public GivenAPaymentServiceIsCalled()
        {
            var account = new Account()
            {
                AccountNumber = "1234-5678",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 500,
                Status = AccountStatus.Live
            };

            mockIDataStore = new Mock<IDataStore>();
            mockIDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            mockIDataStoreFactory = new Mock<IDataStoreFactory>();
            mockIDataStoreFactory.Setup(x => x.CreateDataStore(It.IsAny<string>()))
                .Returns(mockIDataStore.Object);
            mockIConfiguration = new Mock<IConfiguration>();

            mockIAccount = new Mock<IAccount>();
            mockIAccount.SetupGet(x => x.AccountNumber).Returns(account.AccountNumber);
            mockIAccount.SetupGet(x => x.Balance).Returns(account.Balance);
            mockIAccount.SetupGet(x => x.AllowedPaymentSchemes).Returns(account.AllowedPaymentSchemes);
            mockIAccount.SetupGet(x => x.Status).Returns(account.Status);

            systemUnderTest = new PaymentService(mockIDataStore.Object, mockIDataStoreFactory.Object, mockIConfiguration.Object, mockIAccount.Object);
        }

        [Fact]
        public void Constructor_WhenNullIDataStoreIsSupplied_ThenTheExpectedErrorIsThrown()
        {
            var act = () => new PaymentService(null, mockIDataStoreFactory.Object, mockIConfiguration.Object, mockIAccount.Object);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("dataStore");
        }      

        [Fact]
        public void Constructor_WhenNullIDataStoreFactoryIsSupplied_ThenTheExpectedErrorIsThrown()
        {
            var act = () => new PaymentService(mockIDataStore.Object, null, mockIConfiguration.Object, mockIAccount.Object);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("dataStoreFactory");
        }

        [Fact]
        public void Constructor_WhenNullIConfigurationIsSupplied_ThenTheExpectedErrorIsThrown()
        {
            var act = () => new PaymentService(mockIDataStore.Object, mockIDataStoreFactory.Object, null, mockIAccount.Object);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("configuration");
        }

        [Fact]
        public void Constructor_WhenNullIAccountIsSupplied_ThenTheExpectedErrorIsThrown()
        {
            var act = () => new PaymentService(mockIDataStore.Object, mockIDataStoreFactory.Object, mockIConfiguration.Object, null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("account");
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

        [Fact]
        public void MakePayment_WhenAValidRequestAndWithAnyOtherTypeSupplied_ThenTheIDataStoreFactoryIsInvokedWithTheExpectedAccount()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 400,
                CreditorAccountNumber = "Test001",
                DebtorAccountNumber = "Test002",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments,

            };

            mockIDataStoreFactory.Setup(x => x.CreateDataStore("test"))
                .Returns(new AccountDataStore());

            systemUnderTest.MakePayment(request);

            mockIDataStoreFactory.Verify(x => x.CreateDataStore(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void MakePayment_WhenAValidRequestAndWithBackupTypeSupplied_ThenTheIDataStoreFactoryIsInvokedWithTheExpectedAccount()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 400,
                CreditorAccountNumber = "Test001",
                DebtorAccountNumber = "Test002",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments,

            };

            var type = "Backup";
            mockIConfiguration.SetupGet(x => x.DataStoreType).Returns(type);
            mockIDataStoreFactory.Setup(x => x.CreateDataStore(type))
                .Returns(new BackupAccountDataStore());

            systemUnderTest.MakePayment(request);

            mockIDataStoreFactory.Verify(x => x.CreateDataStore(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void MakePayment_WhenValidRequestOnFastPaymentsSupplied_ThenTheExpectedResultIsReturned()
        {
            var request = new MakePaymentRequest()
            {
                Amount = 400,
                CreditorAccountNumber = "Test001",
                DebtorAccountNumber = "Test002",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments,

            };

            var result = systemUnderTest.MakePayment(request);
            result.Success.Should().BeTrue();
        }
    }
}
