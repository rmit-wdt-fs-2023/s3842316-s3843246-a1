using System.Diagnostics;
using MCBA.Login;
using MCBA.Managers;
using MCBA.Model;
using MCBA.Menu.Options;
using MCBA.Utils;
namespace MCBA.Impl.Run;

public class Menu
{
    private readonly CredentialManager _credentialManager;
    private readonly CustomerManager _customerManager;
	private readonly AccountManager _accountManager;
	private readonly TransactionManager _transactionManager;
	private Customer _customer;
	 
	public Menu(CredentialManager credentialManager,
		CustomerManager customerManager, AccountManager accountManager,
		TransactionManager transactionManager)
	{
		_credentialManager = credentialManager;
		_customerManager = customerManager;
		_accountManager = accountManager;
		_transactionManager = transactionManager;
	}

	public void Run()
	{
		var login = new LoginData(_credentialManager, _customerManager);
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
					MiscUtils.PrintErrMsg("Not an Valid Input");
					continue;
				}

				switch (option)
				{
					case 1:
						new Deposit(_accountManager,
							 _transactionManager, _customer).Run();
						break;
					case 2:
						break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
						Run();
                        break;
                    case 6:
						exit = true;
                        break;
					default:
						throw new UnreachableException();
                }
			}
            Console.WriteLine("Good bye!");
        }
        else
			MiscUtils.PrintErrMsg("Missing Data in Database");
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

