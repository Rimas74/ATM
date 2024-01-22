using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using static System.Console;

namespace ATM
    {
    public class User
        {
        private const int MaxLoginAttempts = 3;


        private List<Transaction> transactions;

        [JsonProperty]
        public int RemainingAttempts { get; private set; }
        public bool IsCardBlocked => Card.IsBlocked;
        [JsonProperty]
        public Card Card { get; private set; }
        [JsonProperty]
        public string Username { get; private set; }
        [JsonProperty]
        public string Password { get; private set; }
        [JsonProperty]
        public decimal Balance { get; set; }
        public int LoginAttempts { get; private set; }

        [JsonConstructor]
        public User(Card card, string username, string password, int remainingAttempts = MaxLoginAttempts)
            {
            Card = card;
            Username = username;
            Password = password;
            Balance = 0;
            LoginAttempts = 0;

            RemainingAttempts = remainingAttempts;
            transactions = new List<Transaction>();

            }

        public bool Login(string password)
            {

            if (Card.IsBlocked)
                {

                WriteLine("Your card is blocked");
                return false;
                }

            if (password == Password)
                {
                LoginAttempts = 0;
                RemainingAttempts = MaxLoginAttempts;
                return true;
                }

            else
                {
                LoginAttempts++;
                RemainingAttempts = MaxLoginAttempts - LoginAttempts;

                if (LoginAttempts > MaxLoginAttempts)
                    {
                    BlockCard();
                    }

                return false;
                }
            }


        private void BlockCard()
            {
            Card.IsBlocked = true;
            }

        public decimal ShowBalance()
            {
            return Balance;
            }

        public List<Transaction> ShowTransactions()
            {

            return transactions.OrderByDescending(t => t.Timestamp).Take(5).ToList();
            }

        public bool WithdrawMoney(decimal amount)
            {
            if (!Card.IsBlocked && Balance >= amount)
                {
                Balance -= amount;
                AddTransaction(TransactionType.Withdrawal, amount);
                return true;
                }

            return false;
            }

        public bool Deposit(decimal amount)
            {
            if (!Card.IsBlocked)
                {
                Balance += amount;
                AddTransaction(TransactionType.Deposit, amount);
                return true;
                }

            return false;
            }

        private void AddTransaction(TransactionType type, decimal amount)
            {
            Transaction transaction = new Transaction(type, amount);
            transactions.Add(transaction);
            }
        }
    }

