using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ATM
    {
    public class Transaction
        {
        public int TransactionId { get; }
        public DateTime Timestamp { get; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }

        private static int transactionCounter = 1;

        public Transaction(TransactionType type, decimal amount)
            {
            TransactionId = transactionCounter++;
            Timestamp = DateTime.Now;
            Type = type;
            Amount = amount;
            }
        }
    }
