using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Console;

namespace ATM
    {
    public class ATM
        {
        private List<User> users;
        private List<Transaction> transactions;
        private FileManager fileManager;
        private Dictionary<Guid, int> dailyTransactionCount;


        public ATM()
            {
            users = new List<User>();
            transactions = new List<Transaction>();
            fileManager = new FileManager();
            dailyTransactionCount = new Dictionary<Guid, int>();
            LoadUserData();

            }

        private void LoadUserData()
            {
            users = fileManager.ReadUserData();

            foreach (var user in users)
                {
                user.InitializeDailyTransactions();
                }
            }

        public void Run()
            {
            WriteLine("Welcome to the ATM!");
            while (true)

                {

                WriteLine("Are you a new user or an existing user, or wish to exit? (N for new / E for existing / X for exit the program)");
                string userChoice = ReadLine().ToUpper();

                switch (userChoice)
                    {
                    case "N":
                        User newUser = CreateUser();
                        if (newUser != null)
                            {
                            PerformATMOperations(newUser);
                            }

                        break;

                    case "E":
                        WriteLine("Please enter your username:");
                        string username = ReadLine();
                        string password = PromptForPassword();

                        User currentUser = AuthenticateUser(username, password);
                        if (currentUser != null)
                            {

                            bool loginSuccess = currentUser.Login(password);

                            if (loginSuccess)
                                {
                                PerformATMOperations(currentUser);
                                }

                            else
                                {
                                if (currentUser.Card.IsBlocked)
                                    {
                                    WriteLine("Your card has been blocked due to multiple failed login attempts.");
                                    return;
                                    }
                                else
                                    {
                                    WriteLine($"Login failed. You have {currentUser.RemainingAttempts} attempts left before the card is blocked.");
                                    }
                                }
                            }
                        else
                            {
                            WriteLine("User not found. Please try again or choose a different option.");

                            }
                        fileManager.WriteUserData(users);
                        break;

                    case "X":
                        WriteLine("Thank you for using the ATM. Goodbye!");
                        return;
                    default:
                        WriteLine("Invalid choice. Please try again.");
                        break;
                    }



                }
            }

        private void PerformATMOperations(User currentUser)
            {
            bool continueOperation = true;
            while (continueOperation)
                {
                WriteLine("\nSelect the service required:");
                WriteLine("1. Display balance");
                WriteLine("2. Display last 5 transactions");
                WriteLine("3. Cash withdrawal");
                WriteLine("4. Deposit");
                WriteLine("5. Complete the operation");

                int choice;
                if (int.TryParse(ReadLine(), out choice))
                    {
                    switch (choice)
                        {
                        case 1:
                            ShowBalance(currentUser);
                            break;
                        case 2:
                            ShowTransactions(currentUser);
                            break;
                        case 3:
                            WithdrawMoney(currentUser);
                            break;
                        case 4:
                            DepositMoney(currentUser);
                            break;
                        case 5:
                            continueOperation = false;
                            break;
                        default:
                            WriteLine("Invalid choice. Please try again.");
                            break;
                        }
                    }
                else
                    {
                    WriteLine("Invalid input. Please enter a valid number.");
                    }
                }

            SaveUserData();
            }


        private User AuthenticateUser(string? username, string? password)
            {
            User user = users.Find(u => u.Username == username);
            if (user != null)
                {

                bool loginSuccess = user.Login(password);
                SaveUserData();

                if (loginSuccess)
                    {
                    return user;
                    }

                if (user.Card.IsBlocked)
                    {
                    WriteLine("Your card has been blocked due multiple failed attempts to login.");
                    }
                else
                    {
                    WriteLine($"Login failed. You have {user.RemainingAttempts} attempts left before your card is blocked.");
                    }
                }

            return null;
            }


        private User CreateUser()
            {
            WriteLine("Enter new username:");
            string username = ReadLine();
            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                WriteLine("This username already exists. Please choose a different username.");
                return null;
                }

            WriteLine("Enter new password:");
            string password = PromptForPassword();

            Guid newCardNumber = Guid.NewGuid();
            User newUser = new User(new Card(newCardNumber), username, password);
            users.Add(newUser);
            SaveUserData();
            WriteLine("New user created successfully.");
            return newUser;
            }


        private string PromptForPassword()
            {
            Write("Please enter your password: ");
            string enteredPassword = ReadLine()!;
            WriteLine($"Password entered: {enteredPassword} ");
            return enteredPassword;
            }
        private void WithdrawMoney(User user)
            {
            Write("Please enter the amount to withdraw: ");
            if (decimal.TryParse(ReadLine(), out decimal amount))
                {
                if (amount > 1000)
                    {
                    WriteLine("The withdrawal amount exceeded the maximum limit.");
                    return;
                    }

                if (amount < 0)
                    {
                    WriteLine("Invalid amount. Please enter a positive number.");
                    return;
                    }
                if (user.CanPerformTransaction() && user.WithdrawMoney(amount))
                    {
                    WriteLine($"Withdrawal of {amount:C} successful.");
                    SaveUserData();
                    }
                else
                    {
                    WriteLine("Withdrawal failed. Insufficient funds.");
                    }
                }
            else
                {
                WriteLine("Invalid amount. Please enter a valid number.");
                }
            }

        private void DepositMoney(User user)
            {
            Write("Please enter the amount to deposit: ");
            if (decimal.TryParse(ReadLine(), out decimal amount))
                {
                if (amount < 0)
                    {
                    WriteLine("Invalid amount. Please enter a positive number.");
                    return;
                    }
                if (user.CanPerformTransaction() && user.Deposit(amount))
                    {
                    WriteLine($"Deposit of {amount:C} successful.");
                    SaveUserData();
                    }
                else
                    {
                    WriteLine("Deposit failed. Please try again.");
                    }
                }
            else
                {
                WriteLine("Invalid amount. Please enter a valid number.");
                }
            }
        private void SaveUserData()
            {
            fileManager.WriteUserData(users);
            }




        private void ShowTransactions(User user)
            {
            var recentTransactions = user.ShowTransactions();

            WriteLine("\nRecent Transactions:");
            foreach (Transaction transaction in recentTransactions)
                {
                WriteLine($"Transaction ID: {transaction.TransactionId}, " +
                          $"Type: {transaction.Type}, " +
                          $"Amount: {transaction.Amount:C}");
                }
            }

        private void ShowBalance(User user)
            {

            WriteLine($"Available Balance: {user.ShowBalance():C}");
            }



        }
    }
