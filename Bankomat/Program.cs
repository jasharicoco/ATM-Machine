namespace Bankomat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] usernames = { "user1", "user2", "user3", "user4", "user5" };
            string[] passwords = { "password1", "password2", "password3", "password4", "password5" };

            Console.WriteLine("Dear customer. Welcome to our ATM Machine.");

            // USERNAME
            string username_input;
            bool usernameFound = false;
            int userIndex = 0;

            Console.WriteLine("\nPlease enter your username.");
            while (!usernameFound)
            {
                username_input = Console.ReadLine();

                //Username match-check
                for (int i = 0; i < usernames.Length; i++)
                {
                    if (usernames[i] == username_input)
                    {
                        userIndex = i;
                        usernameFound = true;
                        break;
                    }
                }

                if (!usernameFound)
                {
                    Console.WriteLine("User not found. Try again.");
                }
            }

            // PASSWORD
            string password_input;
            bool passwordCorrect = false;
            int numberOfTries = 0;

            Console.WriteLine("\nPlease enter your password.");
            while (!passwordCorrect && numberOfTries < 3)
            {
                password_input = Console.ReadLine();

                //Password check for indexed user found above
                if (passwords[userIndex] == password_input)
                {
                    passwordCorrect  = true;
                }
                else
                {
                    numberOfTries++;
                    Console.WriteLine($"Password incorrect. Try again. {numberOfTries}/3 tries");
                }
                if (numberOfTries == 3 && !passwordCorrect)
                {
                    Console.WriteLine("You have tried too many times. Take a break.");
                    Environment.Exit(0); //Exits program after 3 wrong entries
                    //Consequence of 3 wrong entries might be changed in future version
                }
            }

            // MENU
            ShowMenu();






            // MODULES
            static void ShowMenu()
            {
                Console.WriteLine("1. See accounts and balance");
                Console.WriteLine("2. Internal transaction (from one of your accounts to another)");
                Console.WriteLine("3. Withdrawal of funds");
                Console.WriteLine("4. Log out");
            }
        }
    }
}
