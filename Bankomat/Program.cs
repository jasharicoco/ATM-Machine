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
            string[][] accounts =
            {
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT", "BROKERAGE ACCOUNT" },   // user1
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT", "FAMILY ACCOUNT" },      // user2
                new string[] { "BROKERAGE ACCOUNT", "PENSION SAVINGS" },                        // user3
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT", "BROKERAGE ACCOUNT" },   // user4
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT" },                        // user5
            };

            decimal[][] accountBalances =
            {
                new decimal[] { 100000m, 500000.69m, -1000m },  // user1
                new decimal[] { 200000m, 10000m },              // user2
                new decimal[] { 1000000m },                     // user3
                new decimal[] { 4500m, 90000m, 5000.86m },      // user4
                new decimal[] { 3000m, 2000m }                  // user5
            };

            int userIndex = 0; // This will represent the index of our logged in user

            bool program = true;
            while (program)
            {
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
                            Transaction(usernames, passwords, accounts, accountBalances, userIndex);
                            break;

                        case 2:
                            MakeWithdrawal(usernames, passwords, accounts, accountBalances, userIndex);
                            break;

                        case 3:
                            MakeDeposit(accounts, accountBalances, userIndex);
                            break;

                        case 4:
                            CrossUserTransaction(usernames, passwords, accounts, accountBalances, userIndex);
                            break;

                        case 5:
                            loggedin = false;
                            Console.WriteLine("\nWelcome back.\nPress any key to continue.\n");
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
            Console.WriteLine("3. Deposit funds");
            Console.WriteLine("4. Send funds to another user");
            Console.WriteLine("5. Log out");
        }
        static void ShowAccountsAndBalances(string[][] accounts, decimal[][] accountBalances, int userIndex)
        {
            Console.WriteLine("\nThese are your accounts and their respective balances.");
            for (int i = 0; i < accounts[userIndex].Length; i++)
            {
                Console.WriteLine($"{i + 1}: {accounts[userIndex][i]}: {accountBalances[userIndex][i]:C}");
            }
        }
        static void Transaction(string[] usernames, string[] passwords, string[][] accounts, decimal[][] accountBalances, int userIndex)
        {
            ShowAccountsAndBalances(accounts, accountBalances, userIndex);
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
                decimal amount = 0;

                Console.WriteLine("\nFrom which account would you like to draw money?");
                while (!Int32.TryParse(Console.ReadLine(), out fromAccount) || fromAccount < 1 || fromAccount > accounts[userIndex].Length)
                {
                    Console.WriteLine("Choose a valid account.");
                }

                Console.WriteLine("\nTo which account would you like to send money?");
                while (!Int32.TryParse(Console.ReadLine(), out toAccount) || toAccount < 1 || toAccount > accounts[userIndex].Length)
                {
                    Console.WriteLine("Choose a valid account.");
                }

                if (fromAccount == toAccount)
                {
                    Console.WriteLine("\nYou cannot make a transaction from/to the same account. Try again.");
                    return; // Exit the method if accounts are the same
                }

                Console.WriteLine("\nHow much would you like to transfer?");
                while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                {
                    Console.WriteLine("Enter a valid amount.");
                }

                if (accountBalances[userIndex][fromAccount - 1] < amount)
                {
                    Console.WriteLine("\nInsufficient funds in the selected account. Try again.");
                }
                if (ConfirmPIN(usernames, passwords, userIndex))
                {
                    // Perform the transaction
                    accountBalances[userIndex][fromAccount - 1] -= amount;
                    accountBalances[userIndex][toAccount - 1] += amount;

                    Console.WriteLine($"\nTransaction successful. {amount.ToString("C")} was sent from {accounts[userIndex][fromAccount - 1]} " +
                        $"to {accounts[userIndex][toAccount - 1]}");

                    Console.WriteLine("\nThese are you new balances.");
                    ShowAccountsAndBalances(accounts, accountBalances, userIndex);
                }
            }
        }
        static void MakeWithdrawal(string[] usernames, string[] passwords, string[][] accounts, decimal[][] accountBalances, int userIndex)
        {
            int fromAccount = 0;
            decimal amount = 0;

            ShowAccountsAndBalances(accounts, accountBalances, userIndex);
            bool withdrawalLoop = true;
            while (withdrawalLoop)
            {
                // Select account to withdraw from
                Console.WriteLine("\nFrom which account would you like to make a withdrawal?");
                while (!Int32.TryParse(Console.ReadLine(), out fromAccount) || fromAccount < 1 || fromAccount > accounts[userIndex].Length)
                {
                    Console.WriteLine("Choose a valid account.");
                }

                while (true)
                {
                    Console.WriteLine("\nHow much would you like to withdraw?");
                    string input = Console.ReadLine();

                    if (input.ToUpper() == "EXIT")
                    {
                        Console.WriteLine("\nTransaction cancelled. Returning to menu.");
                        return;
                    }

                    if (decimal.TryParse(input, out amount) && amount > 0)
                    {
                        // Check if there are sufficient funds
                        if (accountBalances[userIndex][fromAccount - 1] >= amount)
                        {
                            if (ConfirmPIN(usernames, passwords, userIndex))
                            {
                                // Perform the withdrawal
                                accountBalances[userIndex][fromAccount - 1] -= amount;

                                Console.WriteLine($"\nWithdrawal successful. {amount.ToString("C")} ware withdrawn from {accounts[userIndex][fromAccount - 1]}.");

                                Console.WriteLine("\nThese are you new balances.");
                                ShowAccountsAndBalances(accounts, accountBalances, userIndex);
                            }
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
        static void MakeDeposit(string[][] accounts, decimal[][] accountBalances, int userIndex)
        {
            int toAccount = 0;
            decimal amount = 0;

            ShowAccountsAndBalances(accounts, accountBalances, userIndex);
            bool depositLoop = true;
            while (depositLoop)
            {
                // Select account to deposit to

                Console.WriteLine("\nTo which account would you like to deposit funds?");
                while (!Int32.TryParse(Console.ReadLine(), out toAccount) || toAccount < 1 || toAccount > accounts[userIndex].Length)
                {
                    Console.WriteLine("Choose a valid account.");
                }

                while (true)
                {
                    Console.WriteLine("\nHow much would you like to deposit?");
                    string input = Console.ReadLine();

                    if (input.ToUpper() == "EXIT")
                    {
                        Console.WriteLine("\nTransaction cancelled. Returning to menu.");
                        return;
                    }

                    if (decimal.TryParse(input, out amount) && amount > 0)
                    {
                        // Perform the withdrawal
                        accountBalances[userIndex][toAccount - 1] += amount;

                        Console.WriteLine($"\nDeposit successful. {amount.ToString("C")} were deposited to {accounts[userIndex][toAccount - 1]}.");

                        Console.WriteLine("\nThese are you new balances.");
                        ShowAccountsAndBalances(accounts, accountBalances, userIndex);

                        depositLoop = false;
                        break; // Leave loop when transaction is successful
                    }
                    else
                    {
                        Console.WriteLine("Enter a valid amount. Try again or write EXIT to cancel.");
                    }
                }
            }
        }
        static void CrossUserTransaction(string[] usernames, string[] passwords, string[][] accounts, decimal[][] accountBalances, int userIndex)
        {
            int fromAccount = 0;
            int toUser = 0;
            int toAccount = 0;
            decimal amount = 0;

            Console.WriteLine("\nTo which user would you like to send money?");
            for (int i = 0; i < usernames.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {usernames[i]}");
            }

            while (!Int32.TryParse(Console.ReadLine(), out toUser) || toUser < 1 || toUser > usernames.Length)
            {
                Console.WriteLine("Choose a valid user.");
            }

            Console.WriteLine("\nTo which account would you like to transfer the money?");
            for (int i = 0; i < accounts[toUser - 1].Length; i++)
            {
                Console.WriteLine($"{i + 1}: {accounts[toUser - 1][i]}");
            }

            while (!Int32.TryParse(Console.ReadLine(), out toAccount) || toAccount < 1 || toAccount > accounts[toUser - 1].Length)
            {
                Console.WriteLine("Choose a valid account.");
            }


            ShowAccountsAndBalances(accounts, accountBalances, userIndex);
            Console.WriteLine("\nFrom which account would you like to draw money?");
            while (!Int32.TryParse(Console.ReadLine(), out fromAccount) || fromAccount < 1 || fromAccount > accounts[toUser - 1].Length)
            {
                Console.WriteLine("Choose a valid account.");
            }

            Console.WriteLine("\nHow much would you like to transfer?");
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Enter a valid amount.");
            }

            if (accountBalances[userIndex][fromAccount - 1] < amount)
            {
                Console.WriteLine("\nInsufficient funds in the selected account. Try again.");
            }

            if (ConfirmPIN(usernames, passwords, userIndex))
            {
                // Perform the transaction
                accountBalances[userIndex][fromAccount - 1] -= amount;
                accountBalances[toUser - 1][toAccount - 1] += amount;

                Console.WriteLine($"\nTransaction successful. {amount.ToString("C")} was sent from your {accounts[userIndex][fromAccount - 1]} " +
                    $"to {usernames[toUser - 1]}'s {accounts[toUser - 1][toAccount - 1]}.");
                Console.WriteLine("\nThese are you new balances.");

                ShowAccountsAndBalances(accounts, accountBalances, userIndex);
            }
        }
        static bool ConfirmPIN(string[] usernames, string[] passwords, int userIndex)
        {
            {
                int numberOfTries = 0;
                while (numberOfTries < 3)
                {
                    Console.WriteLine("\nConfirm your transaction by entering your password:");
                    string password_input = Console.ReadLine();
                    if (passwords[userIndex] == password_input)
                    {
                        return true; // Exit the method on successful login
                    }
                    // WRONG PIN
                    numberOfTries++;
                    Console.WriteLine($"Incorrect password. {numberOfTries}/3 tries.");
                }
                // THREE WRONG ENTRIES
                Console.WriteLine("\nTransaction cancelled due to incorrect password.");
                return false; // Consequence of 3 wrong entries
            }
        }
    }
}