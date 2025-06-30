using System;
using System.Collections.Generic;

namespace ArkFury.Entities.DTOs
{
    public class Amount
    {
        public string Total { get; set; }
        public string Currency { get; set; }
    }

    public class Transaction
    {
        public Amount Amount { get; set; }
    }

    public class PayPalTransactionDTO
    {
        public string OrderID { get; set; }
        public string Id { get; set; }
        public string Intent { get; set; }
        public DateTime Create_time { get; set; } = DateTime.Now;
        public string State { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string SteamId { get; set; }
        public string ProductDescription { get; set; }
        public string Server { get; set; }
    }
}
