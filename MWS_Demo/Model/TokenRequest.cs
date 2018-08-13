namespace MWS_Demo.Model
{
    public class TokenRequest
    {
        public decimal Amount { get; set; }
        public string RequestDateTime { get; set; }
        public string CurrencyID { get; set; }
        public string MerchantSalesID { get; set; }
        public string TrakingCode { get; set; }
        public string FilterBy { get; set; }
        public string TransactionOkURL { get; set; }
        public string CustomMerchantName { get; set; }
        public int ExpirationTime { get; set; }
        public int TransactionExpirationTime { get; set; }
        public int ProductID { get; set; }
    }
}
