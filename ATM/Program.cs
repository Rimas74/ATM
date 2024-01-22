using static System.Console;
namespace ATM
    {
    internal class Program
        {
        static void Main(string[] args)
            {
            //Guid cardNumber = Guid.NewGuid();
            //Card newCard = new Card(cardNumber);

            //string newPassword = "Zigmas1974!";

            //User newUser = new User(newCard, newPassword);

            ATM atm = new ATM();
            atm.Run();

            WriteLine("ATM simulation completed. Press any key to exit.");
            ReadKey();
            }
        }
    }
