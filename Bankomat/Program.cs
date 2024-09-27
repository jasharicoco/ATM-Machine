using System.Reflection.Metadata;

namespace Bankomat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // USERNAME AND PASSWORD STORAGE
            string[] usernames = { "a", "user2", "user3", "user4", "user5" };
            string[] passwords = { "a", "password2", "password3", "password4", "password5" };

            // ACCOUNTS & BALANCE STORAGE
            object[,,] accounts = {
            { // user1 accounts and balances
                { "CHECKING ACCOUNT", 1000 },
                { "SAVINGS ACCCOUNT", 2000 },
                { "BROKERAGE ACCOUNT", 3000 },
            },
            { // user2 accounts and balances
                { "CHECKING ACCOUNT", 1000 },
                { "SAVINGS ACCCOUNT", 2000 },
                { "BROKERAGE ACCOUNT", 3000 },
            },
            { // user3 accounts and balances
                { "CHECKING ACCOUNT", 1000 },
                { "SAVINGS ACCCOUNT", 2000 },
                { "BROKERAGE ACCOUNT", 3000 },
            },
            { // user4 accounts and balances
                { "CHECKING ACCOUNT", 1000 },
                { "SAVINGS ACCCOUNT", 2000 },
                { "BROKERAGE ACCOUNT", 3000 },
            },
            { // user5 accounts and balances
                { "CHECKING ACCOUNT", 1000 },
                { "SAVINGS ACCCOUNT", 2000 },
                { "BROKERAGE ACCOUNT", 3000 },
            },
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

            int userIndex = 0; // Index for our logged in user
            for (int i = 0; i < usernames.Length; i++)
            {
                if (loggedInUser == usernames[i])
                {
                    userIndex = i; break;
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
                    case 1:
                        ShowAccountsAndBalances(accounts, userIndex);
                        Console.WriteLine("\nWould you like to make an internal transaction?\n1. Yes\n2. No");
                        bool transactionLoop = true;
                        while (transactionLoop)
                        {
                            Int32.TryParse(Console.ReadLine(), out int transactionChoice);
                            switch (transactionChoice)
                            {
                                case 1:
                                    bool transaction = false;
                                    while (!transaction)
                                    {
                                        Console.WriteLine("\nFrom which account would you like to draw money?" +
                                            "\nChoose account 1, 2 or 3.");
                                        if (Int32.TryParse(Console.ReadLine(), out int fromAccount) && fromAccount >= 1 && fromAccount <= 3)
                                        {
                                            Console.WriteLine("\nTo which account would you like to send money?" +
                                                "\nChoose account 1,2 or 3.");
                                            if (Int32.TryParse(Console.ReadLine(), out int toAccount) && toAccount >= 1 && toAccount <= 3)
                                            {
                                                if (fromAccount == toAccount)
                                                {
                                                    Console.WriteLine("\nYou cannot make a transaction from/to the same account. Try again.");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\nHow much would you like to transfer?");
                                                    if (Int32.TryParse(Console.ReadLine(), out int amount) && amount > 0)
                                                    {
                                                        if ((int)accounts[userIndex, fromAccount - 1, 1] >= amount)
                                                        {
                                                            accounts[userIndex, fromAccount - 1, 1] = (int)accounts[userIndex, fromAccount - 1, 1] - amount;
                                                            accounts[userIndex, toAccount - 1, 1] = (int)accounts[userIndex, toAccount - 1, 1] + amount;

                                                            ShowAccountsAndBalances(accounts, userIndex);
                                                            transaction = true;
                                                            transactionLoop = false;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("\nInsufficient funds in the selected account. Try again.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\nEnter a valid amount.");
                                                    }
                                                }
                                            }
                                            else
                                            { Console.WriteLine("\nChoose a valid number."); }
                                        }
                                        else
                                        { Console.WriteLine("\nChoose a valid number."); }
                                    }
                                    ShowMenu();
                                    transactionLoop = false;
                                    break;

                                case 2:
                                    ShowMenu();
                                    transactionLoop = false;
                                    break;

                                default: Console.WriteLine("\nChoose one of the above."); break;
                            }
                        }
                        break;

                    case 2:
                        break;

                    case 3:
                        menu = false;
                        Console.WriteLine("Welcome back.\nPress any key to continue.");
                        Console.ReadKey();
                        break;

                    default:
                        Console.WriteLine("\nChoose one of the above.");
                        break;
                }
            }
        }

        // METHOD STORAGE
        static void ShowMenu()
        {
            Console.WriteLine("\n1. See account and balances and/or make internal transaction");
            Console.WriteLine("2. Withdrawal of funds");
            Console.WriteLine("3. Log out");
        }
        static void ShowAccountsAndBalances(object[,,] accounts, int userIndex)
        {
            Console.WriteLine("\nThese are your accounts and their respective balances.");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[userIndex, i, 0]} {accounts[userIndex, i, 1]}");
            }
        }
    }
}