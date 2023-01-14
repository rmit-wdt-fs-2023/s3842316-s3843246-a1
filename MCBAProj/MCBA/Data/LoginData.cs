using MCBA.Managers;
using MCBA.Model;
using MCBA.Utils;
using SimpleHashing.Net;
namespace MCBA.Login;

public class LoginData
{
    private readonly CredentialManager _credentialManager;
    private readonly CustomerManager _customerManager;
    private int _loginId;
    private string _passwordHash;
    private Credential _credential;

    public LoginData(DBManagerFactory manager)
    {
        _credentialManager = manager._credentialManager;
        _customerManager = manager._customerManager;
    }

    public void ReadAndValidate()
    {
        while (true)
        {
            // User Input
            Console.Write("Enter Login Id: ");
            var inputId = Console.ReadLine();

            // If unable to parse int continue
            if (!int.TryParse(inputId, out var loginId))
            {
                MiscUtils.PrintErrMsg("LoginID must be an Integer");
                continue;
            }
            _loginId = loginId;

            // Password
            Console.Write("Enter Password: ");
            /*
			 * ConsoleKey code Adapted from: 
			 * https://stackoverflow.com/questions/3404421/password-masking-console-application
			 */
            string tempPass = string.Empty;
            ConsoleKey keyRead;
            do
            {
                var keyInfo = Console.ReadKey(true);
                keyRead = keyInfo.Key;

                // Backspace Handeling
                if (keyRead == ConsoleKey.Backspace && tempPass.Length > 0)
                {
                    Console.Write("\b \b");
                    tempPass = tempPass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    tempPass += keyInfo.KeyChar;
                }
            } while (keyRead != ConsoleKey.Enter);
            // Code Adaped till here


            // Validation
            if (tempPass == string.Empty)
            {
                Console.WriteLine();
                MiscUtils.PrintErrMsg("Password can't be empty");
                continue;
            }
            _passwordHash = tempPass;

            // If password is valid loop breaks
            if (ValidatePassword())
                break;

            Console.WriteLine();
            MiscUtils.PrintErrMsg("LoginID and Password combination is not found");
        }
    }

    private bool ValidatePassword()
    {
        Credential credential = _credentialManager.GetCredentials(_loginId);
        if (credential == null)
            return false;

        if (!new SimpleHash().Verify(_passwordHash, credential.PasswordHash))
            return false;

        _credential = credential;
        return true;
    }

    public Customer GetCustomer() => _customerManager.GetCustomer(_credential);
}