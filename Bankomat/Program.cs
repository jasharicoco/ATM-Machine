using System.Reflection.Metadata;

namespace Bankomat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // USERNAME AND PASSWORD STORAGE
            string[] usernames = { "user1", "user2", "user3", "user4", "user5" };
            string[] passwords = { "password1", "password2", "password3", "password4", "password5" };

            // ACCOUNTS & BALANCE STORAGE
            decimal[,] accounts =
            {
                { 1000, 2000, 3000 }, //user1 accounts
                { 1000, 2000, 3000 }, //user2 accounts
                { 1000, 2000, 3000 }, //user3 accounts
                { 1000, 2000, 3000 }, //user4 accounts
                { 1000, 2000, 3000 }  //user5 accounts
            };

            Console.WriteLine("Dear customer. Welcome to our ATM Machine.");

            bool loginSuccessful = false;
            int numberOfTries = 0;
            string loggedInUser = ""; // This will represent the name of our logged in user

            while (!loginSuccessful && numberOfTries < 3)
            {
                // USERNAME AND PASSWORD INPUT
                Console.WriteLine("\nPlease enter your username:");
                string username_input = Console.ReadLine();

                Console.WriteLine("Please enter your password:");
                string password_input = Console.ReadLine();

                // CHECK COMPATIBILITY
                for (int i = 0; i < usernames.Length; i++)
                {
                    // Password is case-sensitive while username is not.
                    if (usernames[i].ToUpper() == username_input.ToUpper() && passwords[i] == password_input)
                    {
                        loggedInUser = usernames[i];
                        loginSuccessful = true;
                        break;
                    }
                }

                if (!loginSuccessful)
                {
                    numberOfTries++;
                    Console.WriteLine($"Incorrect username or password combination. Try again. {numberOfTries}/3 tries.");
                }

                if (numberOfTries == 3 && !loginSuccessful)
                {
                    Console.WriteLine("You have tried too many times. Take a break.");
                    Environment.Exit(0); // Consequence of 3 wrong entries
                }
            }

            Console.WriteLine($"\nAccess granted!\nWelcome, {loggedInUser}.");
            ShowMenu();

            bool menu = true;
            while (menu)
            {
                Int32.TryParse(Console.ReadLine(), out int choice);
                switch (choice)
                {
                    case 1: break;

                    case 2: break;

                    case 3: break;

                    case 4: break;

                    default: Console.WriteLine("Choose one of the above."); ; break;
                }
            }
        }

        // METHOD STORAGE
        static void ShowMenu()
        {
            Console.WriteLine("\n1. See accounts and balance");
            Console.WriteLine("2. Internal transaction (from one of your accounts to another)");
            Console.WriteLine("3. Withdrawal of funds");
            Console.WriteLine("4. Log out");
        }
    }
}

// CODE THAT WILL BE USEFUL LATER
// Console.WriteLine(accounts[0, 1].ToString("C")); // show value in currency