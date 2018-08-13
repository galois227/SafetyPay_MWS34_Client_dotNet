using MWS_Demo.MWS34;
using System;

namespace MWS_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var mwsProxy = new MWSProxy();
                //OperationResponse operationResponse = mwsProxy.GetOperation("01234567890123456");
                ExpressTokenResponse expressTokenResponse = mwsProxy.CreateExpressToken(new Model.TokenRequest()
                {
                    Amount = 12.50M,
                    CurrencyID = "PEN",
                    CustomMerchantName = "M_TEST",
                    ExpirationTime = 120,
                    FilterBy = "",
                    MerchantSalesID = "TEST",
                    ProductID = 1,
                    RequestDateTime = "2018-01-25T19:49:57",
                    TrakingCode = "",
                    TransactionExpirationTime = 120,
                    TransactionOkURL = "https://www.google.com"
                });

                string ss = "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
