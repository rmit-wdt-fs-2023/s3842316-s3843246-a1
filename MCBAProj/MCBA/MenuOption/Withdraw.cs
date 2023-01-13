using MCBA.Model;
using MCBA.Managers;
using MCBA.Menu.Option;
using static MCBA.Utils.MiscUtils;
using static MCBA.Utils.ConstValues;
using static MCBA.Utils.TransactionMath;

namespace MCBA.Menu.Options;

public class Withdraw : AbstractTransactions
{
    public Withdraw(AccountManager accountManager,
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
            WithdrawMoney(_accounts[--option]);
        }
    }

    private void WithdrawMoney(Account account)
    {
        decimal availableBalance = ComputeAvailableBalance(account);
        if (GetNoOfTransactions(account.AccountNumber) >= 2 && availableBalance != 0)
            availableBalance -= WithdrawFee;

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
                WithdrawBackendCalls(account, (decimal)amount, comment);
        }
    }

    private void WithdrawBackendCalls(Account account, decimal amount,
        string comment)
    {
        var transaction = new Transaction()
        {
            TransactionType = ((char)TransactionType.Withdraw),
            AccountNumber = account.AccountNumber,
            DestinationAccountNumber = null,
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
                Amount = WithdrawFee,
                Comment = null,
                TransactionTimeUtc = DateTime.Now
            };

            accountNewBalance = accountNewBalance.ComputeWithdrawBalance(WithdrawFee);
            _transactionManager.InsertTransaction(serviceCharge);
            _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);
        }
        if (GetNoOfTransactions(account.AccountNumber) >= 2 && accountNewBalance != 0)
            accountNewBalance -= WithdrawFee;
        Console.WriteLine($"Withdraw of {amount:C} successful, account balance now {accountNewBalance:C}");
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
                if ((amount + WithdrawFee) > ComputeAvailableBalance(account))
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
                if ((amount + WithdrawFee) > ComputeAvailableBalance(account))
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
        Console.WriteLine("--- Withdraw ---");
        for (var i = 0; i < _customer.Accounts.Capacity; i++)
        {
            Console.WriteLine($"{i + 1}. {_accounts[i].AccountType.GetAccStrFromChar()}\t" +
                $"{_accounts[i].AccountNumber}\t{_accounts[i].Balance:C}");
        }
        Console.Write("\nSelect an account: ");
    }

}