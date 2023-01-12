using MCBA.Login;
using MCBA.Managers;
using MCBA.Model;
using MCBA.Utils;
namespace MCBA.Impl.Run;

public class Menu
{
    private readonly CredentialManager _credentialManager;
    private readonly CustomerManager _customerManager;
	private Customer _customer;
	 
	public Menu(CredentialManager credentialManager, CustomerManager customerManager)
	{
		_credentialManager = credentialManager;
		_customerManager = customerManager;
	}

	public void Run()
	{
		var login = new LoginData(_credentialManager, _customerManager);
		login.ReadAndValidate();
		_customer = login.GetCustomer();

		// Checks if customer is not null
		if (_customer != null)
		{
			while (true)
			{
				PrintMenu();
				var usrOption = Console.ReadLine();
			}
		}
		else
			MiscUtils.PrintErrMsg("Missing Data in Database");
	}

	private void PrintMenu()
	{
		Console.WriteLine(
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

