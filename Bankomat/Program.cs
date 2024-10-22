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
            string[] usernames = { "Adam", "Filip", "Petter", "Daniel", "Alex" };
            string[] passwords = { "adam", "filip", "petter", "daniel", "alex" };

            // ACCOUNTS & BALANCE STORAGE
            string[][] accounts =
            {
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT", "BROKERAGE ACCOUNT" },   // user1
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT" },                        // user2
                new string[] { "BROKERAGE ACCOUNT" },                                           // user3
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT", "BROKERAGE ACCOUNT" },   // user4
                new string[] { "CHECKING ACCOUNT", "SAVINGS ACCCOUNT" },                        // user5
            };

            decimal[][] balances =
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
                Console.Clear();
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
                            Transaction(usernames, passwords, accounts, balances, userIndex);
                            break;

                        case 2:
                            MakeWithdrawal(usernames, passwords, accounts, balances, userIndex);
                            break;

                        case 3:
                            MakeDeposit(accounts, balances, userIndex);
                            break;

                        case 4:
                            CrossUserTransaction(usernames, passwords, accounts, balances, userIndex);
                            break;

                        case 5:
                            loggedin = false;
                            Console.WriteLine("\nWelcome back.\nPress any key to continue.\n");
                            Console.ReadKey();
                            break;

                            case 6:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid choice.");
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

                if (username_input == "EXIT")
                {
                    Environment.Exit(0);
                }

                // PASSWORD INPUT
                Console.WriteLine("Please enter your password:");
                string password_input = ReadPassword();

                // CHECK COMPATIBILITY
                for (int i = 0; i < usernames.Length; i++)
                {
                    // Password is case-sensitive while username is not
                    if (usernames[i].ToUpper() == username_input.ToUpper() && passwords[i] == password_input)
                    {
                        Console.Clear();
                        userIndex = i; // Store the user index
                        Console.WriteLine($"Access granted! Welcome, {usernames[i]}.");

                        return; // Exit the method on successful login
                    }
                }

                // WRONG COMBINATION
                numberOfTries++;
                Console.Clear();
                Console.WriteLine($"Incorrect username or password combination. {numberOfTries}/3 tries.\nType EXIT if you wish to quit.");

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
            Console.WriteLine("\n1. Make an internal transaction");
            Console.WriteLine("2. Withdraw funds");
            Console.WriteLine("3. Deposit funds");
            Console.WriteLine("4. Send funds to another user");
            Console.WriteLine("5. Log out");
            Console.WriteLine("6. Exit the application");
        }
        static void ShowAccountsAndBalances(string[][] accounts, decimal[][] balances, int userIndex)
        {
            Console.WriteLine("These are your accounts and their respective balances.");
            for (int i = 0; i < accounts[userIndex].Length; i++)
            {
                Console.WriteLine($"{i + 1}: {accounts[userIndex][i]}: {balances[userIndex][i]:C}");
            }
        }
        static void Transaction(string[] usernames, string[] passwords, string[][] accounts, decimal[][] balances, int userIndex)
        {
            Console.Clear();
            ShowAccountsAndBalances(accounts, balances, userIndex);

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
                Console.Clear();
                Console.WriteLine("You cannot make a transaction from/to the same account. Try again.");
                return; // Exit the method if accounts are the same
            }

            Console.WriteLine("\nHow much would you like to transfer?");
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Enter a valid amount.");
            }

            if (balances[userIndex][fromAccount - 1] < amount)
            {
                Console.Clear();
                Console.WriteLine("Insufficient funds in the selected account. Try again.");
            }

            else if (ConfirmPIN(usernames, passwords, userIndex))
            {
                // Perform the transaction
                balances[userIndex][fromAccount - 1] -= amount;
                balances[userIndex][toAccount - 1] += amount;

                Console.Clear();
                Console.WriteLine($"Transaction successful. {amount.ToString("C")} was sent from {accounts[userIndex][fromAccount - 1]} " +
                    $"to {accounts[userIndex][toAccount - 1]}");
                Console.WriteLine($"New balance for {accounts[userIndex][fromAccount - 1]}: {balances[userIndex][fromAccount - 1].ToString("C")}");
                Console.WriteLine($"New balance for {accounts[userIndex][toAccount - 1]}: {balances[userIndex][toAccount - 1].ToString("C")}");
            }
        }
        static void MakeWithdrawal(string[] usernames, string[] passwords, string[][] accounts, decimal[][] balances, int userIndex)
        {
            int fromAccount = 0;
            decimal amount = 0;

            Console.Clear();
            ShowAccountsAndBalances(accounts, balances, userIndex);
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

                    if (decimal.TryParse(input, out amount) && amount > 0)
                    {
                        // Check if there are sufficient funds
                        if (balances[userIndex][fromAccount - 1] >= amount)
                        {
                            if (ConfirmPIN(usernames, passwords, userIndex))
                            {
                                // Perform the withdrawal
                                balances[userIndex][fromAccount - 1] -= amount;

                                Console.Clear();
                                Console.WriteLine($"Withdrawal successful. {amount.ToString("C")} were withdrawn from {accounts[userIndex][fromAccount - 1]}.");

                                Console.WriteLine($"New balance for {accounts[userIndex][fromAccount - 1]}: {balances[userIndex][fromAccount - 1].ToString("C")}");
                            }
                            withdrawalLoop = false;
                            break; // Leave loop when transaction is successful
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Insufficient funds in the selected account. Try again.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Enter a valid amount.");
                    }
                }
            }
        }
        static void MakeDeposit(string[][] accounts, decimal[][] balances, int userIndex)
        {
            int toAccount = 0;
            decimal amount = 0;

            Console.Clear();
            ShowAccountsAndBalances(accounts, balances, userIndex);
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

                    if (decimal.TryParse(input, out amount) && amount > 0)
                    {
                        // Perform the withdrawal
                        balances[userIndex][toAccount - 1] += amount;

                        Console.Clear();
                        Console.WriteLine($"Deposit successful. {amount.ToString("C")} were deposited to {accounts[userIndex][toAccount - 1]}.");
                        Console.WriteLine($"New balance for {accounts[userIndex][toAccount - 1]}: {balances[userIndex][toAccount - 1].ToString("C")}");

                        depositLoop = false;
                        break; // Leave loop when transaction is successful
                    }
                    else
                    {
                        Console.WriteLine("Enter a valid amount.");
                    }
                }
            }
        }
        static void CrossUserTransaction(string[] usernames, string[] passwords, string[][] accounts, decimal[][] balances, int userIndex)
        {
            int fromAccount = 0;
            int toUser = 0;
            int toAccount = 0;
            decimal amount = 0;

            Console.Clear();
            Console.WriteLine("To which user would you like to send money?");
            for (int i = 0; i < usernames.Length; i++)
            {
                if (i != userIndex) // Exclude the logged in user from the list...
                {
                    Console.WriteLine($"{i + 1}: {usernames[i]}");
                }
            }

            while (!Int32.TryParse(Console.ReadLine(), out toUser) || toUser < 1 || toUser > usernames.Length || toUser == userIndex + 1)
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

            Console.WriteLine("\nFrom which account would you like to draw money?\n");
            ShowAccountsAndBalances(accounts, balances, userIndex);
            while (!Int32.TryParse(Console.ReadLine(), out fromAccount) || fromAccount < 1 || fromAccount > accounts[userIndex].Length)
            {
                Console.WriteLine("Choose a valid account.");
            }

            Console.WriteLine("\nHow much would you like to transfer?");
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Enter a valid amount.");
            }

            if (balances[userIndex][fromAccount - 1] < amount)
            {
                Console.Clear();
                Console.WriteLine("Insufficient funds in the selected account. Try again.");
            }

            else if (ConfirmPIN(usernames, passwords, userIndex))
            {
                // Perform the transaction
                balances[userIndex][fromAccount - 1] -= amount;
                balances[toUser - 1][toAccount - 1] += amount;

                Console.Clear();
                Console.WriteLine($"Transaction successful. {amount.ToString("C")} was sent from your {accounts[userIndex][fromAccount - 1]} " +
                    $"to {usernames[toUser - 1]}'s {accounts[toUser - 1][toAccount - 1]}.");
                Console.WriteLine($"New balance for {accounts[userIndex][fromAccount - 1]}: {balances[userIndex][fromAccount - 1].ToString("C")}");
            }
        }
        static bool ConfirmPIN(string[] usernames, string[] passwords, int userIndex)
        {
            {
                int numberOfTries = 0;
                while (numberOfTries < 3)
                {
                    Console.WriteLine("\nConfirm your transaction by entering your password:");
                    string password_input = ReadPassword();
                    if (passwords[userIndex] == password_input)
                    {
                        return true; // Exit the method on successful login
                    }
                    // WRONG PIN
                    numberOfTries++;
                    Console.WriteLine($"\nIncorrect password. {numberOfTries}/3 tries.");
                }
                // THREE WRONG ENTRIES
                Console.Clear();
                Console.WriteLine("Transaction cancelled due to incorrect password.");
                return false; // Consequence of 3 wrong entries
            }
        }
        static string ReadPassword()
        {
            //This method is straight up copy-paste from the web. I have implemented it's function, and I understand how it works.
            //However, I could not have written this from scratch. Not yet.
            string password = string.Empty;

            while (true)
            {
                // Read a single character
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                // Check if the key is Enter
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                // Check if the key is Backspace
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        // Remove the last character from the password
                        password = password[..^1];
                        // Move the cursor back, overwrite the '*' and move back again
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    // Add the character to the password
                    password += keyInfo.KeyChar;
                    // Display an asterisk
                    Console.Write('*');
                }
            }
            return password;
        }
    }
}