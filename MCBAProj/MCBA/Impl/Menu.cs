using MCBA.Logic;
using MCBA.Managers;
using MCBA.Model;
namespace MCBA.Impl.Run;

public class Menu
{
    private readonly CredentialManager _credentialManager;
    private readonly CustomerManager _customerManager;
	private Credential _credential;

	public Menu(CredentialManager credentialManager, CustomerManager customerManager)
	{
		_credentialManager = credentialManager;
		_customerManager = customerManager;
	}

	public void Run()
	{
		_credential = new Login(_credentialManager).ReadAndValidate();

		// Debug 
		Console.WriteLine("Success");
		Console.ReadLine();
        MainMenu();
	}

	private void MainMenu()
	{

	}

	
}

