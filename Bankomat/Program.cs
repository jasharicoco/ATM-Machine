using System.Reflection.Metadata;
using System.Runtime;
using System.Security.Principal;

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
                { "CHECKING ACCOUNT", 22322,78 },
                { "SAVINGS ACCCOUNT", 120000,12 },
                { "BROKERAGE ACCOUNT", 80000,00 },
            },
            { // user2 accounts and balances
                { "CHECKING ACCOUNT", 14720,02 },
                { "SAVINGS ACCCOUNT", 40000,00 },
                { "BROKERAGE ACCOUNT", 140800,20 },
            },
            { // user3 accounts and balances
                { "CHECKING ACCOUNT", 42420,42 },
                { "SAVINGS ACCCOUNT", 18292,00 },
                { "BROKERAGE ACCOUNT", 19020,02 },
            },
            { // user4 accounts and balances
                { "CHECKING ACCOUNT", 4042,02 },
                { "SAVINGS ACCCOUNT", 8220,92 },
                { "BROKERAGE ACCOUNT", 22180,20 },
            },
            { // user5 accounts and balances
                { "CHECKING ACCOUNT", 2420,95 },
                { "SAVINGS ACCCOUNT", 1529,28 },
                { "BROKERAGE ACCOUNT", 5120,20 },
            },
            };

            int userIndex = 0; // This will represent the index of our logged in user

            bool program = true;
            while (program)
            {
                Console.WriteLine("");
                Console.WriteLine("\x1b[1mDear customer. Welcome to our ATM Machine.\x1b[0m");

                Login(usernames, passwords, ref userIndex);

                bool loggedin = true;
                while (loggedin)
                {
                    ShowMenu();
                    Int32.TryParse(Console.ReadLine(), out int choice);
                    switch (choice)
                    {
                        case 1:
                            Transaction(accounts, userIndex);
                            break;

                        case 2:
                            MakeWithdrawal(accounts, userIndex);
                            break;

                        case 3:
                            loggedin = false;
                            Console.WriteLine("Welcome back.\nPress any key to continue.\n");
                            Console.ReadKey();
                            break;

                        default:
                            Console.WriteLine("\nChoose one of the above.");
                            break;
                    }
                }
            }
        }

        // METHOD STORAGE
        static void Login(string[] usernames, string[] passwords, ref int userIndex)
        {
            int numberOfTries = 0;
            while (numberOfTries < 3)
            {
                // USERNAME INPUT
                Console.WriteLine("\nPlease enter your username:");
                string username_input = Console.ReadLine();

                // PASSWORD INPUT
                Console.WriteLine("Please enter your password:");
                string password_input = Console.ReadLine();

                // CHECK COMPATIBILITY
                for (int i = 0; i < usernames.Length; i++)
                {
                    // Password is case-sensitive while username is not
                    if (usernames[i].ToUpper() == username_input.ToUpper() && passwords[i] == password_input)
                    {
                        userIndex = i; // Store the user index
                        Console.WriteLine($"\nAccess granted!\nWelcome, {usernames[i]}.");

                        return; // Exit the method on successful login
                    }
                }

                // WRONG COMBINATION
                numberOfTries++;
                Console.WriteLine($"Incorrect username or password combination. {numberOfTries}/3 tries.");

                // THREE WRONG ENTRIES
                if (numberOfTries == 3 && true)
                {
                    Console.WriteLine("Come back when you're sober.");
                    Environment.Exit(0); // Consequence of 3 wrong entries
                }
            }
        }
        static void ShowMenu()
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. See account and balances and/or make internal transaction");
            Console.WriteLine("2. Withdraw funds");
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
        static void Transaction(object[,,] accounts, int userIndex)
        {
            ShowAccountsAndBalances(accounts, userIndex);
            Console.WriteLine("\nWould you like to make an internal transaction?\n1. Yes\n2. No");
            int transactionChoice = 0;

            while (!Int32.TryParse(Console.ReadLine(), out transactionChoice) || transactionChoice < 1 || transactionChoice > 2)
            {
                Console.WriteLine("\nChoose one of the above.");
            }

            if (transactionChoice == 2)
            {
                return; // Exit the method if the user doesn't want to transact
            }

            if (transactionChoice == 1)
            {
                int fromAccount = 0;
                int toAccount = 0;
                int amount = 0;

                Console.WriteLine("\nFrom which account would you like to draw money?");
                while (!Int32.TryParse(Console.ReadLine(), out fromAccount) || fromAccount < 1 || fromAccount > 3)
                {
                    Console.WriteLine("Choose a valid account.");
                }

                Console.WriteLine("\nTo which account would you like to send money?");
                while (!Int32.TryParse(Console.ReadLine(), out toAccount) || toAccount < 1 || toAccount > 3)
                {
                    Console.WriteLine("Choose a valid account.");
                }

                if (fromAccount == toAccount)
                {
                    Console.WriteLine("\nYou cannot make a transaction from/to the same account. Try again.");
                    return; // Exit the method if accounts are the same
                }

                Console.WriteLine("\nHow much would you like to transfer?");
                while (!Int32.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                {
                    Console.WriteLine("Enter a valid amount.");
                }

                if ((int)accounts[userIndex, fromAccount - 1, 1] < amount)
                {
                    Console.WriteLine("\nInsufficient funds in the selected account. Try again.");
                }
                else
                {
                    // Perform the transaction
                    accounts[userIndex, fromAccount - 1, 1] = (int)accounts[userIndex, fromAccount - 1, 1] - amount;
                    accounts[userIndex, toAccount - 1, 1] = (int)accounts[userIndex, toAccount - 1, 1] + amount;

                    Console.WriteLine("\nTransaction successful. These are you new balances.\n");

                    ShowAccountsAndBalances(accounts, userIndex);
                }
            }
        }
        static void MakeWithdrawal(object[,,] accounts, int userIndex)
        {
            int fromAccount = 0;
            int amount = 0;

            ShowAccountsAndBalances(accounts, userIndex);
            bool withdrawalLoop = true;
            while (withdrawalLoop)
            {
                // Select account to withdraw from
                while (true)
                {
                    Console.WriteLine("\nFrom which account would you like to make a withdrawal?");
                    if (Int32.TryParse(Console.ReadLine(), out fromAccount) && fromAccount >= 1 && fromAccount <= 3)
                    {
                        break; // Leave loop with correct input
                    }
                    Console.WriteLine("Choose a valid account.");
                }

                while (true)
                {
                    Console.WriteLine("\nHow much would you like to withdraw?");
                    string input = Console.ReadLine();

                    if (input.ToUpper() == "EXIT")
                    {
                        Console.WriteLine("\nTransaction cancelled. Returning to menu.");
                        ShowMenu();
                        return;
                    }

                    if (Int32.TryParse(input, out amount) && amount > 0)
                    {
                        // Check if there are sufficient funds
                        if ((int)accounts[userIndex, fromAccount - 1, 1] >= amount)
                        {
                            // Perform the withdrawal
                            accounts[userIndex, fromAccount - 1, 1] = (int)accounts[userIndex, fromAccount - 1, 1] - amount;
                            Console.WriteLine("\nWITHDRAWAL SUCCESSFUL.");

                            ShowAccountsAndBalances(accounts, userIndex);
                            ShowMenu();
                            withdrawalLoop = false;
                            break; // Leave loop when transaction is successful
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds. Try again or write EXIT to cancel.");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Enter a valid amount. Try again or write EXIT to cancel.");
                    }
                }
            }
        }
    }
}