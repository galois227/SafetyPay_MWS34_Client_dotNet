using MWS_Demo.Model;
using MWS_Demo.MWS34;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace MWS_Demo
{
    public class MWSProxy
    {
        private string _signatureKey;
        private string _apiKey;

        #region contructor

        public MWSProxy()
        {
            _apiKey = AppSettings.AppKey;
            _signatureKey = AppSettings.SignatureKey;
        }

        #endregion

        #region public methods

        public OperationResponse GetOperation(string merchantSalesID)
        {
            string dateTime = DateTime.Now.ToString("s");

            StringBuilder plainTextMessage = new StringBuilder();
            plainTextMessage.Append(dateTime);
            plainTextMessage.Append(merchantSalesID);
            plainTextMessage.Append(_signatureKey);

            using (MerchantExpressApiOperationsClient client = new MerchantExpressApiOperationsClient())
            {
                return client.GetOperation(new OperationRequest()
                {
                    ApiKey = _apiKey,
                    MerchantSalesID = merchantSalesID,
                    RequestDateTime = dateTime,
                    Signature = ComputeHash(plainTextMessage.ToString())
                });
            }
        }

        public ExpressTokenResponse CreateExpressToken(TokenRequest tokenRequest)
        {
            using (MerchantExpressApiOperationsClient client = new MerchantExpressApiOperationsClient())
            {
                return client.CreateExpressToken(GetRequest(tokenRequest));
            }
        }

        public TransactionSpecIdentifierResponse VoidTransaction()
        {
            using (MerchantExpressApiOperationsClient client = new MerchantExpressApiOperationsClient())
            {
                return client.VoidTransaction(new TransactionSpecIdentifierRequest() { });
            }
        }

        #endregion

        #region private methods

        private ExpressTokenRequest GetRequest(TokenRequest tokenRequest)
        {
            StringBuilder sb = new StringBuilder()
                .Append(tokenRequest.RequestDateTime)
                .Append(tokenRequest.CurrencyID)
                .Append(tokenRequest.Amount)
                .Append(tokenRequest.MerchantSalesID)
                .Append(LanguageCodeType.ES)
                .Append(tokenRequest.TrakingCode)
                .Append(tokenRequest.ExpirationTime)
                .Append(tokenRequest.TransactionOkURL)
                .Append(tokenRequest.TransactionOkURL)
                .Append(_signatureKey);

            return new ExpressTokenRequest()
            {
                ApiKey = _apiKey,
                RequestDateTime = tokenRequest.RequestDateTime,
                CurrencyID = tokenRequest.CurrencyID,
                Amount = tokenRequest.Amount,
                MerchantSalesID = tokenRequest.MerchantSalesID,
                Language = LanguageCodeType.ES,
                LanguageSpecified = true,
                TrackingCode = tokenRequest.TrakingCode,
                ExpirationTime = tokenRequest.ExpirationTime.ToString(),
                FilterBy = tokenRequest.FilterBy,
                TransactionOkURL = tokenRequest.TransactionOkURL,
                TransactionErrorURL = tokenRequest.TransactionOkURL,
                TransactionExpirationTime = tokenRequest.TransactionExpirationTime.ToString(),
                CustomMerchantName = tokenRequest.CustomMerchantName,
                ProductID = tokenRequest.ProductID.ToString(),
                LocalizedCurrencyID = tokenRequest.CurrencyID,
                Signature = ComputeHash(sb.ToString()),
                ShopperInformation = GetShopperInformation()
            };
        }

        private static ShopperFieldType[] GetShopperInformation()
        {
            return new List<ShopperFieldType>() {
                new ShopperFieldType() {Key= "Email",Value="gflores@safetypay.com"},
                new ShopperFieldType() {Key="FirstName",Value="Galois" },
                new ShopperFieldType() {Key="LastName",Value="Flores" },
                new ShopperFieldType() {Key = "Skype" ,Value = "gflores_safetypay"},
                new ShopperFieldType() {Key = "Address",Value = "Ate" }
            }.ToArray();
        }

        private string ComputeHash(string pstrMessage)
        {
            byte[] plainTextWithSaltBytes = Encoding.UTF8.GetBytes(pstrMessage);
            SHA256Managed hash = new SHA256Managed();
            byte[] hashBytes;
            hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            return ConvertBytesToHexaString(hashBytes);
        }

        private string ConvertBytesToHexaString(byte[] bytes)
        {
            StringBuilder hexString = new StringBuilder(bytes.Length);
            for (int i = 0; (i <= (bytes.Length - 1)); i++)
            {
                hexString.Append(bytes[i].ToString("X2"));
            }
            return hexString.ToString();
        }

        #endregion
    }

    public static class AppSettings
    {
        public static string AppKey = ConfigurationManager.AppSettings["AppKey"];
        public static string SignatureKey = ConfigurationManager.AppSettings["SignatureKey"];
    }
}
