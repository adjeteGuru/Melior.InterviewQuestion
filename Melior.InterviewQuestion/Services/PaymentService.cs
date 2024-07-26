using Melior.InterviewQuestion.Types;
using System;
using System.Configuration;

namespace Melior.InterviewQuestion.Services
{
    public class PaymentService : IPaymentService
    {
        private IDataStore dataStore;
        private readonly IDataStoreFactory dataStoreFactory;

        public PaymentService(IDataStore dataStore, IDataStoreFactory dataStoreFactory)
        {
            this.dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            this.dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            Account account = null;
           
            dataStore = dataStoreFactory.CreateDataStore(dataStoreType);

            account = dataStore.GetAccount(request.DebtorAccountNumber);

           
            var result = GeneratePaymentResultOnRequest(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;                

                dataStore = dataStoreFactory.CreateDataStore(dataStoreType);

                dataStore.UpdateAccount(account);
            }

            return result;
        }

        private static MakePaymentResult GeneratePaymentResultOnRequest(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();
           
            if (account != null)
            {
                switch (request.PaymentScheme)
                {
                    case PaymentScheme.Bacs:
                        result.Success = account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
                        break;

                    case PaymentScheme.FasterPayments:
                        result.Success = account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
                            && account.Balance >= request.Amount;
                        break;

                    case PaymentScheme.Chaps:
                        result.Success = account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)
                            && account.Status == AccountStatus.Live;
                        break;
                }
            }

            result.Success = false;            

            return result;
        }
    }
}
