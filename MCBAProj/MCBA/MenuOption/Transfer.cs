using MCBA.Model;
using MCBA.Managers;
using MCBA.Menu.Option;
using static MCBA.Utils.MiscUtils;
using static MCBA.Utils.ConstValues;
using static MCBA.Utils.TransactionMath;
namespace MCBA.Menu.Options;

public class Transfer : AbstractTransactions
{
    private Account _destAccount;

    public Transfer(DBManagerFactory manager, Customer customer) :
        base(manager, customer) { }

    public override void Run()
    {
        Console.WriteLine();
        PrintMenu();
        var usrInput = Console.ReadLine();
        if (int.TryParse(usrInput, out var option) && option.IsInRange(1, _accounts.Capacity))
        {
            TransferMoney(_accounts[--option]);
        }
    }

    // Calls required methods
    private void TransferMoney(Account account)
    {
        int? destAccountNumber = ValidateDestinationAccount(account);
        if (destAccountNumber != null)
        {
            decimal availableBalance = ComputeAvailableBalance(account);
            if (GetNoOfTransactions(account.AccountNumber) >= 2 && availableBalance != 0)
                availableBalance -= TransferFee;

            Console.Write(
                $"{account.AccountType.GetAccStrFromChar()} {account.AccountNumber}, " +
                $"Balance: {account.Balance:C}, Available Balance: {availableBalance:C}" +
                $"\nEnter amount: ");

            decimal? amount = AmountValidation(account);
            if (amount != null)
            {
                string comment = GetCommentInput();
                if (comment != null && comment.Length > MaxCommentLenght)
                    PrintErrMsg("Comment exceeded maximun length");

                if (comment == null || comment.Length <= MaxCommentLenght)
                {
                    if (_destAccount != null)
                    {
                        TransferBackendCalls((decimal)amount, comment);

                        WithdrawBackendCalls(account, (decimal)amount, comment);
                    }
                }
            }
        }
    }

    // Withdraw database calls
    private void WithdrawBackendCalls(Account account, decimal amount,
        string comment)
    {

        // Backend Calls
        var transaction = new Transaction()
        {
            TransactionType = ((char)TransactionType.Transfer),
            AccountNumber = account.AccountNumber,
            DestinationAccountNumber = _destAccount.AccountNumber,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        };

        decimal accountNewBalance = account.Balance.ComputeWithdrawBalance(amount);
        _transactionManager.InsertTransaction(transaction);
        _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);


        if (ServiceFeeRequired(account.AccountNumber))
        {
            var serviceCharge = new Transaction()
            {
                TransactionType = ((char)TransactionType.ServiceCharge),
                AccountNumber = account.AccountNumber,
                DestinationAccountNumber = null,
                Amount = TransferFee,
                Comment = null,
                TransactionTimeUtc = DateTime.Now
            };

            accountNewBalance = accountNewBalance.ComputeWithdrawBalance(TransferFee);
            _transactionManager.InsertTransaction(serviceCharge);
            _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);
        }

        if (GetNoOfTransactions(account.AccountNumber) >= 2 && accountNewBalance != 0)
            accountNewBalance -= TransferFee;
        Console.WriteLine($"Transfer of {amount:C} successful, account balance now {accountNewBalance:C}");
    }

    // Transfer database calls
    private void TransferBackendCalls(decimal amount, string comment)
    {

        var transaction = new Transaction()
        {
            TransactionType = ((char)TransactionType.Transfer),
            AccountNumber = _destAccount.AccountNumber,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        };

        decimal accountNewBalance = _destAccount.Balance.ComputeDepositBalance(amount);
        _transactionManager.InsertTransaction(transaction);
        _accountManager.UpdateBalance(_destAccount.AccountNumber, accountNewBalance);
    }

    // Checks if destination account exists
    private int? ValidateDestinationAccount(Account account)
    {
        Console.Write($"Enter destination account number: ");
        var usrInput = Console.ReadLine();

        if (!int.TryParse(usrInput, out var destAccountNumber))
        {
            PrintErrMsg("Not an Valid Input");
            return null;
        }
        else if (destAccountNumber == account.AccountNumber)
        {
            PrintErrMsg("Destination account cannot be the same account");
            return null;
        }

        foreach (var destAccount in _accountManager.All())
        {
            if (destAccount.AccountNumber == destAccountNumber)
            {
                _destAccount = destAccount;
                return destAccountNumber;
            }
        }

        PrintErrMsg("Destination account Not found");
        return null;

    }

    private decimal? AmountValidation(Account account)
    {
        var usrInput = Console.ReadLine();

        // Input validation
        if (!decimal.TryParse(usrInput, out decimal amount))
        {
            PrintErrMsg("Invalid Input");
            return null;
        }
        else if (amount == 0)
        {
            PrintErrMsg("Amount cannot be zero");
            return null;
        }
        else if (amount < 0)
        {
            PrintErrMsg("Amount cannot be negative");
            return null;
        }
        else if (account.AccountType == ((char)AccountType.Checking))
        {
            if (GetNoOfTransactions(account.AccountNumber) >= 2)
            {
                if ((amount + TransferFee) > ComputeAvailableBalance(account))
                {
                    PrintErrMsg("Ammount cannot be greater than available balance");
                    return null;
                }
            }
            else if (amount > ComputeAvailableBalance(account))
            {
                PrintErrMsg("Ammount cannot be greater than available balance");
                return null;
            }
        }
        else if ((account.AccountType == ((char)AccountType.Saving)))
        {
            if (GetNoOfTransactions(account.AccountNumber) >= 2)
            {
                if ((amount + TransferFee) > ComputeAvailableBalance(account))
                {
                    PrintErrMsg("Ammount cannot be greater than available balance");
                    return null;
                }
            }
        }

        return amount;
    }

    protected override void PrintMenu()
    {
        Console.WriteLine("--- Transfer ---");
        for (var i = 0; i < _customer.Accounts.Capacity; i++)
        {
            Console.WriteLine($"{i + 1}. {_accounts[i].AccountType.GetAccStrFromChar()}\t" +
                $"{_accounts[i].AccountNumber}\t{_accounts[i].Balance:C}");
        }
        Console.Write("\nSelect an account: ");
    }

}