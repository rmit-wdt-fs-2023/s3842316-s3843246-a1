using System;

namespace MCBA;

public class MCBA
{
    private static void Main()
    {

        MCBA mcba = new MCBA();
        mcba.Login();
        int option = 0;

        //ConsoleKeyInfo cki;


        while (option != 6)
		{

			Console.Write("\n");
			var menu =
					"""
					--- Rish Rao ---
					[1] Deposit
					[2] Withdraw
					[3] Transfer
					[4] My statement
					[5] Logout
					[6] Exit

					Enter an option: 
					""";


			Console.Write(menu);
            option = int.Parse(Console.ReadLine());
            switch (option)
			{
				case 1:

					break;
				case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Logged out successfully");
                    mcba.Login();
                    break;
                //case 6:
					//System.Environment.Exit(0);
     //               break;
            }
			
		}

	}

	public void Login()
	{
        string savedusername = "RishRao";
        string savedpassword = "123";
        string username = "";
        string password = "";
        bool login = false;
        ConsoleKeyInfo key;


        while (!login)
        {

            do
            {
                Console.Write("Enter Login ID: ");
                //var temp = Console.ReadLine();

                if (!String.IsNullOrWhiteSpace(username = Console.ReadLine()))
                {

                }
                else
                {
                    Console.WriteLine("username cannot be empty or contain whitespaces");
                }
            } while (String.IsNullOrEmpty(username));

            Console.Write("Enter Password: ");
            do
            {
                key = Console.ReadKey(true);


                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                    //Console.WriteLine(password);

                }
                else
                {
                    if (password.Length > 0 && key.Key == ConsoleKey.Backspace)
                    {
                        Console.Write("\b \b");
                        password = password[0..(password.Length - 1)];

                    }
                }

                //username = Console.ReadLine();
                //Console.Write(username);
            }
            while (key.Key != ConsoleKey.Enter);


            if (username == savedusername && password == savedpassword)
            {
                Console.WriteLine("\n Login Success");
                login = true;
            }
            else
            {
                Console.WriteLine(username + " " + password + " " + username.Length + " " + password.Length);
                Console.WriteLine("Incorrect username or password, please retry.");

            }
        }
    }
		
}

	


