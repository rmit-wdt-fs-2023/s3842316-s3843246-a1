using MCBA.Model;
using MCBA.Managers;
using MCBA.Utils;
using MCBA.Menu.Option;
using static MCBA.Utils.ConstValues;

namespace MCBA.Menu.Options;

public class Transfer : AbstractTransactions
{
    private Account _destAccount;

    public Transfer(AccountManager accountManager,
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
            TransferMoney(_accounts[--option]);
        }
    }

    private void TransferMoney(Account account)
    {
        int? destAccountNumber = ValidateDestinationAccount(account);
        if (destAccountNumber != null)
        {
            decimal availableBalance = TransactionMath.ComputeAvailableBalance(account);
            if (GetNoOfTransactions(account.AccountNumber) >= 2 && availableBalance != 0)
                availableBalance -= ConstValues.TransferFee;

            Console.Write(
                $"{account.AccountType.GetAccStrFromChar()} {account.AccountNumber}, " +
                $"Balance: {account.Balance:C}, Available Balance: {availableBalance:C}" +
                $"\nEnter amount: ");

            decimal? amount = AmountValidation(account);
            if (amount != null)
            {
                string comment = GetCommentInput();
                if (comment != null && comment.Length > ConstValues.MaxCommentLenght)
                    MiscUtils.PrintErrMsg("Comment exceeded maximun length");

                if (comment == null || comment.Length <= ConstValues.MaxCommentLenght)
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

    private void WithdrawBackendCalls(Account account, decimal amount,
        string comment) {

        // Backend Calls
        var transaction = new Transaction()
        {
            TransactionType = ((char)ConstValues.TransactionType.Transfer),
            AccountNumber = account.AccountNumber,
            DestinationAccountNumber = _destAccount.AccountNumber,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Today
        };

        decimal accountNewBalance = account.Balance.ComputeWithdrawBalance(amount);
        _transactionManager.InsertTransaction(transaction);
        _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);


        if (ServiceFeeRequired(account.AccountNumber))
        {
            var serviceCharge = new Transaction()
            {
                TransactionType = ((char)ConstValues.TransactionType.ServiceCharge),
                AccountNumber = account.AccountNumber,
                DestinationAccountNumber = null,
                Amount = ConstValues.TransferFee,
                Comment = null,
                TransactionTimeUtc = DateTime.Today
            };

            accountNewBalance = accountNewBalance.ComputeWithdrawBalance(ConstValues.TransferFee);
            _transactionManager.InsertTransaction(serviceCharge);
            _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);
        }

        if (GetNoOfTransactions(account.AccountNumber) >= 2 && accountNewBalance != 0)
            accountNewBalance -= ConstValues.TransferFee;
        Console.WriteLine($"Transfer of {amount:C} successful, account balance now {accountNewBalance:C}");
    }

    private void TransferBackendCalls(decimal amount, string comment)
    {

        var transaction = new Transaction()
        {
            TransactionType = ((char)ConstValues.TransactionType.Transfer),
            AccountNumber = _destAccount.AccountNumber,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Today
        };

        decimal accountNewBalance = _destAccount.Balance.ComputeDepositBalance(amount);
        _transactionManager.InsertTransaction(transaction);
        _accountManager.UpdateBalance(_destAccount.AccountNumber, accountNewBalance);
    }

    private int? ValidateDestinationAccount(Account account)
    {
        Console.Write($"Enter destination account number: ");
        var usrInput = Console.ReadLine();

        if (!int.TryParse(usrInput, out var destAccountNumber))
        {
            MiscUtils.PrintErrMsg("Not an Valid Input");
            return null;
        }
        else if (destAccountNumber == account.AccountNumber)
        {
            MiscUtils.PrintErrMsg("Destination account cannot be the same account");
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

        MiscUtils.PrintErrMsg("Destination account Not found");
        return null;

    }

    private decimal? AmountValidation(Account account)
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
        else if (account.AccountType == ((char)ConstValues.AccountType.Checking))
        {
            if (GetNoOfTransactions(account.AccountNumber) >= 2)
            {
                if ((amount + ConstValues.TransferFee) > TransactionMath.ComputeAvailableBalance(account))
                {
                    MiscUtils.PrintErrMsg("Ammount cannot be greater than available balance");
                    return null;
                }
            }
            else if (amount > TransactionMath.ComputeAvailableBalance(account))
            {
                MiscUtils.PrintErrMsg("Ammount cannot be greater than available balance");
                return null;
            }
        }
        else if ((account.AccountType == ((char)ConstValues.AccountType.Saving)))
        {
            if (GetNoOfTransactions(account.AccountNumber) >= 2)
            {
                if ((amount + ConstValues.TransferFee) > TransactionMath.ComputeAvailableBalance(account))
                {
                    MiscUtils.PrintErrMsg("Ammount cannot be greater than available balance");
                    return null;
                }
            }
        }

        return amount;
    }



    private void PrintMenu()
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