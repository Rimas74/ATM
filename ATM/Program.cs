using static System.Console;
namespace ATM
    {
    internal class Program
        {
        static void Main(string[] args)
            {

            ATM atm = new ATM();
            atm.Run();

            WriteLine("ATM simulation completed. Press any key to exit.");
            ReadKey();
            }
        }
    }
