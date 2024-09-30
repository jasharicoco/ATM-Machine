using System.Reflection.Metadata;
using System.Runtime;

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
                { "CHECKING ACCOUNT", 42000 },
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
                                    Transaction(accounts, userIndex);
                                    ShowMenu();
                                    transactionLoop = false;
                                    break;

                                case 2:
                                    ShowMenu();
                                    transactionLoop = false;
                                    break;

                                default:
                                    Console.WriteLine("\nChoose one of the above.");
                                    break;
                            }
                        }
                        break;

                    case 2:
                        MakeWithdrawal(accounts, userIndex);
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
        static void Transaction(object[,,] accounts, int userIndex)
        {
            int fromAccount = 0;
            int toAccount = 0;
            int amount = 0;

            // Select account to withdraw from
            while (true)
            {
                Console.WriteLine("\nFrom which account would you like to draw money?");
                if (Int32.TryParse(Console.ReadLine(), out fromAccount) && fromAccount >= 1 && fromAccount <= 3)
                {
                    break; // Leave loop with correct user input
                }
                Console.WriteLine("Choose a valid account.");
            }

            // Select account to transfer to
            while (true)
            {
                Console.WriteLine("\nTo which account would you like to transfer money?");
                if (Int32.TryParse(Console.ReadLine(), out toAccount) && toAccount >= 1 && toAccount <= 3)
                {
                    if (fromAccount == toAccount)
                    {
                        Console.WriteLine("\nYou cannot make a transaction from/to the same account. Try again.");
                        continue;
                    }
                    break; // Leave loop with correct user input
                }
                Console.WriteLine("Choose a valid account.");
            }

            // Enter amount to transfer
            while (true)
            {
                Console.WriteLine("\nHow much would you like to transfer?");
                string input = Console.ReadLine();

                if (input.ToUpper() == "EXIT")
                {
                    Console.WriteLine("\nTransaction cancelled. Returning to menu.");
                    return;
                }

                if (Int32.TryParse(input, out amount) && amount > 0)
                {
                    // Check if there are sufficient funds
                    if ((int)accounts[userIndex, fromAccount - 1, 1] >= amount)
                    {
                        // Perform the transaction
                        accounts[userIndex, fromAccount - 1, 1] = (int)accounts[userIndex, fromAccount - 1, 1] - amount;
                        accounts[userIndex, toAccount - 1, 1] = (int)accounts[userIndex, toAccount - 1, 1] + amount;

                        Console.WriteLine("\nTransaction successful.");

                        ShowAccountsAndBalances(accounts, userIndex);
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
                    Console.WriteLine("Enter a valid amount.");
                }
            }
        }
        static void MakeWithdrawal(object[,,] accounts, int userIndex)
        {
            ShowAccountsAndBalances(accounts, userIndex);
            Console.WriteLine("\nFrom which account would you like to make a withdrawal?");
            bool withdrawal = false;
            while (!withdrawal)
            {
                if (Int32.TryParse(Console.ReadLine(), out int withdrawalAccount) && withdrawalAccount >= 1 && withdrawalAccount <= 3)
                {
                    Console.WriteLine("\nHow much would you like to withdraw?");

                    if (!Int32.TryParse(Console.ReadLine(), out int withdrawalAmount) || withdrawalAmount < 0)
                    {
                        Console.WriteLine("Enter a valid amount.");
                    }

                    else if (withdrawalAmount > (int)accounts[userIndex, withdrawalAccount - 1, 1])
                    {
                        Console.WriteLine("\nInsufficient funds on account.\nTry again (choose account to withdraw from).");
                    }

                    else
                    {
                        accounts[userIndex, withdrawalAccount - 1, 1] = (int)accounts[userIndex, withdrawalAccount - 1, 1] - withdrawalAmount;
                        Console.WriteLine("\nWithdrawal successful.");
                        ShowAccountsAndBalances(accounts, userIndex);
                        ShowMenu();
                        withdrawal = true;
                    }
                }
                else { Console.WriteLine("\nChoose one of your accounts."); }
        }
    }
}
}