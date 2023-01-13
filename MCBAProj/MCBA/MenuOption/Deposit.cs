using MCBA.Model;
using MCBA.Managers;
using MCBA.Utils;
using MCBA.Menu.Option;
namespace MCBA.Menu.Options;

public class Deposit : AbstractTransactions
{
    public Deposit(AccountManager accountManager,
        TransactionManager transactionManager, Customer customer) :
        base(accountManager, transactionManager, customer)
    { }

    public override void Run()
    {
        Console.WriteLine();
        PrintMenu();
        var usrInput = Console.ReadLine();
        if (int.TryParse(usrInput, out var option) && option.IsInRange(1, _accounts.Capacity))
        {
            DepositMoney(_accounts[--option]);
        }
    }

    private void DepositMoney(Account account)
    {
        Console.Write(
            $"{account.AccountType.GetAccStrFromChar()} {account.AccountNumber}, " +
            $"Balance: {account.Balance:C}, Available Balance: {TransactionMath.ComputeAvailableBalance(account):C}" +
            $"\nEnter amount: ");

        decimal? amount = AmountValidation();
        if (amount != null)
        {
            string comment = GetCommentInput();
            if (comment != null)
                if (comment.Length > ConstValues.MaxCommentLenght)
                    MiscUtils.PrintErrMsg("Comment exceeded maximun length");

            if (comment == null || comment.Length <= ConstValues.MaxCommentLenght)
                DepositBackendCalls(account, (decimal)amount, comment);
        }
    }

    private void DepositBackendCalls(Account account, decimal amount,
        string comment)
    {
        // Backend Calls
        var transaction = new Transaction()
        {
            TransactionType = ((char)ConstValues.TransactionType.Deposit),
            AccountNumber = account.AccountNumber,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Today
        };

        decimal accountNewBalance = account.Balance.ComputeDepositBalance(amount);
        _transactionManager.InsertTransaction(transaction);
        _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);

        Console.WriteLine($"Deposit of {amount:C} successful, account balance now {accountNewBalance:C}");
    }

    private decimal? AmountValidation()
    {
        var usrInput = Console.ReadLine();

        // Input validation
        if (!decimal.TryParse(usrInput, out decimal amount))
        {
            MiscUtils.PrintErrMsg("Invalid Input");
            return null;
        }
        else if (amount == 0)
        {
            MiscUtils.PrintErrMsg("Amount cannot be zero");
            return null;
        }
        else if (amount < 0)
        {
            MiscUtils.PrintErrMsg("Amount cannot be negative");
            return null;
        }

        if (amount % 1 != 0)
        {
            string decimalString = amount.ToString();
            int decimalPlacesLength = (decimalString.Substring(decimalString.IndexOf(".")).Length) - 1;

            if (decimalPlacesLength > 2)
            {
                MiscUtils.PrintErrMsg("Amount cannot have more than 2 decimal places");
                return null;
            }
        }
        return amount;
    }

    private void PrintMenu()
    {
        Console.WriteLine("--- Deposit ---");
        for (var i = 0; i < _customer.Accounts.Capacity; i++)
        {
            Console.WriteLine($"{i + 1}. {_accounts[i].AccountType.GetAccStrFromChar()}\t" +
                $"{_accounts[i].AccountNumber}\t{_accounts[i].Balance:C}");
        }
        Console.Write("\nSelect an account: ");
    }
}

