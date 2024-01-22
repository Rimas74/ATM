using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Console;

namespace ATM
    {
    public class User
        {
        private List<Transaction> transactions;
        public Card Card { get; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public decimal Balance { get; set; }
        public int LoginAttempts { get; private set; }

        public User(Card card, string username, string password)
            {
            Card = card;
            Username = username;
            Password = password;
            Balance = 0;
            LoginAttempts = 0;
            transactions = new List<Transaction>();

            }

        public bool Login(string password)
            {
            WriteLine($"Entered password: {password}");
            WriteLine($"Actual password: {Password}");
            WriteLine($"Card is blocked: {Card.IsBlocked}");

            if (!Card.IsBlocked && password == Password)
                {
                LoginAttempts = 0;
                return true;
                }
            else
                {
                LoginAttempts++;
                WriteLine($"Login failed. Attempts: {LoginAttempts}, Card Blocked: {Card.IsBlocked}");
                if (LoginAttempts > 10)
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
