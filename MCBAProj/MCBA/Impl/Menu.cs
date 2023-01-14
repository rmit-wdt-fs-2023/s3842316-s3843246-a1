using System.Diagnostics;
using MCBA.Login;
using MCBA.Managers;
using MCBA.Model;
using MCBA.Menu.Options;
using MCBA.Menu.Option;
using static MCBA.Utils.MiscUtils;
using static MCBA.Utils.ConstValues;
namespace MCBA.Impl.Run;

public class Menu
{
	private readonly DBManagerFactory _manager;
	private Customer? _customer;

	public Menu(DBManagerFactory managers) => _manager = managers;

	public void Run()
	{
		var login = new LoginData(_manager);
		login.ReadAndValidate();
		_customer = login.GetCustomer();

		Console.WriteLine("\n");

		// Checks if customer is not null
		if (_customer != null)
		{
			var exit = false;
			while (!exit)
			{
				_customer = login.GetCustomer();
				Console.WriteLine();
				PrintMenu();
				var usrInput = Console.ReadLine();

				if(!int.TryParse(usrInput, out var option) || !option.IsInRange(1,6))
				{
					PrintErrMsg("Not an Valid Input");
					continue;
				}

                switch (option)
				{
					case 1:
						new Deposit(_manager, _customer).Run();
						break;
					case 2:
                        new Withdraw(_manager, _customer).Run();
                        break;
                    case 3:
						new Transfer(_manager, _customer).Run();
                        break;
                    case 4:
                        new MyStatements(_manager, _customer).Run();
                        break;
                    case 5:
						_customer = null;
						login = null;
						exit = true;
						Console.Clear();
                        Run();
						continue;
                    case 6:
                        _customer = null;
                        login = null;
						exit = true;
                        Console.Clear();
						Console.WriteLine("Program ending.");
                        continue;
					default:
						throw new UnreachableException();
                }
			}
        }
        else
			PrintErrMsg("Missing Data in Database");
	}

	private void PrintMenu()
	{
		Console.Write(
			$"""
			--- {_customer.Name} ---
			[1] Deposit
			[2] Withdraw
			[3] Transfer
			[4] My Statement
			[5] Logout
			[6] Exit

			Enter an option: 
			""");
	}
}

