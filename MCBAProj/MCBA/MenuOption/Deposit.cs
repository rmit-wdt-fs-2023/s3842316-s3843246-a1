using MCBA.Model;
using MCBA.Managers;
using MCBA.Utils;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MCBA.Menu.Options;

public class Deposit
{
	private readonly AccountManager _accountManager;
	private readonly TransactionManager _transactionManager;
	private Customer _customer;
	private List<Account> _accounts;

	public Deposit(AccountManager accountManager,
		TransactionManager transactionManager, Customer customer)
	{
		_accountManager = accountManager;
		_transactionManager = transactionManager;
		_customer = customer;
		_accounts = _customer.Accounts;
	}

	public void Run()
	{
		Console.WriteLine();
		var exit = false;
		while (!exit)
		{
            PrintMenu();
            var usrInput = Console.ReadLine();
            if (!int.TryParse(usrInput, out var option) || !option.IsInRange(1, _accounts.Capacity))
            {
				exit = true;
                continue;
            }

			exit = DepositMoney(_accounts[--option]);
        }
	}

	private bool DepositMoney(Account account)
    {
		Console.Write(
			$"{account.AccountType.GetAccStrFromChar()} {account.AccountNumber}, " +
			$"Balance: {account.Balance:C}, Available Balance: {AvailableBalance(account):C}" +
			$"\nEnter amount: ");
		var usrInput = Console.ReadLine();

		// Input validation
        if (!decimal.TryParse(usrInput, out decimal amount))
        {
            MiscUtils.PrintErrMsg("Invalid Input");
			return true;
        } else if(amount == 0) 
		{
            MiscUtils.PrintErrMsg("Amount cannot be zero");
            return true;
        } else if(amount < 0)
		{
            MiscUtils.PrintErrMsg("Amount cannot be negative");
            return true;
        }

		if (amount % 1 != 0)
		{
			string decimalString = amount.ToString();
			int decimalPlacesLength = (decimalString.Substring(decimalString.IndexOf(".")).Length) - 1;

			if (decimalPlacesLength > 2)
            {
                MiscUtils.PrintErrMsg("Amount cannot have more than 2 decimal places");
                return true;
            }
        }

		Console.Write("Enter comment (n to quit, max length 30: ");
		string comment = "";
		var keyInfo = Console.ReadKey(true);
        var keyRead = keyInfo.Key;

		if (keyRead == ConsoleKey.N)
		{
			// Checks is key like shift is pressed so no uppercase
			if (!char.IsControl(keyInfo.KeyChar))
				comment = null;
		} else if(comment == string.Empty)
		{
			Console.Write(keyInfo.KeyChar);
			comment = keyRead.ToString() + Console.ReadLine();
		}

		if(comment != null)
			if (comment.Length > ConstValues.MaxCommentLenght)
			{
				MiscUtils.PrintErrMsg("Comment exceeded maximun length");
				return true;
			}

        var transaction = new Transaction()
		{
			TransactionType = ((char)ConstValues.TransactionType.Deposit),
			AccountNumber = account.AccountNumber,
			DestinationAccountNumber = null,
			Amount = amount,
			Comment = comment,
			TransactionTimeUtc = DateTime.Today

        };

		decimal newBalance = (account.Balance + amount);
		_transactionManager.InsertTransaction(transaction);
		_accountManager.UpdateBalance(account.AccountNumber, (newBalance));
		Console.WriteLine($"Deposit of {amount:C} successful, account balance now {newBalance:0.00}");
		return true;
    }

    private void PrintMenu()
	{
		Console.WriteLine("--- Deposit ---");
		for(var i = 0; i < _customer.Accounts.Capacity; i++)
		{
			Console.WriteLine($"{i+1}. {_accounts[i].AccountType.GetAccStrFromChar()}\t" +
				$"{_accounts[i].AccountNumber}\t{_accounts[i].Balance:C}");
		}
		Console.Write("\nSelect an account: ");
	}

	private decimal AvailableBalance(Account account)
	{
		if (account.AccountType == ((char)ConstValues.AccountType.Saving))
			return account.Balance;
		else
			return (account.Balance - ConstValues.CreditAccountMinBalance) <=
				0 ? 0 : (account.Balance - ConstValues.CreditAccountMinBalance);
    }
}

