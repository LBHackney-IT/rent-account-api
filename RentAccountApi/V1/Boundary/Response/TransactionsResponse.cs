using System.Collections.Generic;
using Newtonsoft.Json;

namespace RentAccountApi.V1.Boundary.Response
{
    public class TransactionsResponse
    {
        public string Status { get; set; }
        [JsonProperty("request")]
        public TransactionRequest TransactionRequest { get; set; }
        [JsonProperty("transactions")]
        public List<LBHTransactions> Transactions { get; set; }
        public List<string> Errors { get; set; }
    }

    public class LBHTransactions
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("in")]
        public string valueIn { get; set; }
        [JsonProperty("out")]
        public string valueOut { get; set; }
        [JsonProperty("balance")]
        public string Balance { get; set; }
    }

    public class TransactionRequest
    {
        [JsonProperty("paymentRef")]
        public string PaymentRef { get; set; }
        [JsonProperty("postCode")]
        public string PostCode { get; set; }
    }
}
