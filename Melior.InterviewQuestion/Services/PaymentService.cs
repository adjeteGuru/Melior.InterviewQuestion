using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;
using System.Configuration;

namespace Melior.InterviewQuestion.Services
{
    public class PaymentService : IPaymentService
    {
        private IDataStore dataStore;
        private readonly IDataStoreFactory dataStoreFactory;

        public PaymentService(IDataStore dataStore, IDataStoreFactory dataStoreFactory)
        {
            this.dataStore = dataStore;
            this.dataStoreFactory = dataStoreFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            Account account = null;

            //if (dataStoreType == "Backup")
            //{
            //    var accountDataStore = new BackupAccountDataStore();
            //    account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            //}
            //else
            //{
            //    var accountDataStore = new AccountDataStore();
            //    account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            //}
            dataStore = dataStoreFactory.CreateDataStore(dataStoreType);

            account = dataStore.GetAccount(request.DebtorAccountNumber);

            //var result = new MakePaymentResult();

            //switch (request.PaymentScheme)
            //{
            //    case PaymentScheme.Bacs:
            //        if (account == null)
            //        {
            //            result.Success = false;
            //        }
            //        else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            //        {
            //            result.Success = false;
            //        }
            //        break;

            //    case PaymentScheme.FasterPayments:
            //        if (account == null)
            //        {
            //            result.Success = false;
            //        }
            //        else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            //        {
            //            result.Success = false;
            //        }
            //        else if (account.Balance < request.Amount)
            //        {
            //            result.Success = false;
            //        }
            //        break;

            //    case PaymentScheme.Chaps:
            //        if (account == null)
            //        {
            //            result.Success = false;
            //        }
            //        else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            //        {
            //            result.Success = false;
            //        }
            //        else if (account.Status != AccountStatus.Live)
            //        {
            //            result.Success = false;
            //        }
            //        break;
            //}

            var result = GeneratePaymentResultOnRequest(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;

                if (dataStoreType == "Backup")
                {
                    var accountDataStore = new BackupAccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    var accountDataStore = new AccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
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
