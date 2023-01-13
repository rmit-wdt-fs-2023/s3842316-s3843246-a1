using MCBA.Model;
using MCBA.Managers;
using MCBA.Utils;
using MCBA.Menu.Option;
using static MCBA.Utils.ConstValues;

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
        Console.Write(
            $"{account.AccountType.GetAccStrFromChar()} {account.AccountNumber}, " +
            $"Balance: {account.Balance:C}, Available Balance: {TransactionMath.ComputeAvailableBalance(account):C}" +
            $"\nEnter amount: ");

        decimal? amount = AmountValidation(account);

        if (amount != null)
        {
            string comment = GetCommentInput();
            if (comment != null && comment.Length > ConstValues.MaxCommentLenght)
                    MiscUtils.PrintErrMsg("Comment exceeded maximun length");

            if(comment == null || comment.Length <= ConstValues.MaxCommentLenght)
            {
                // Backend Calls
                decimal amountVal = (decimal)amount;

                var transaction = new Transaction()
                {
                    TransactionType = ((char)ConstValues.TransactionType.Withdraw),
                    AccountNumber = account.AccountNumber,
                    DestinationAccountNumber = null,
                    Amount = amountVal,
                    Comment = comment,
                    TransactionTimeUtc = DateTime.Today
                };

                decimal accountNewBalance = account.Balance.ComputeWithdrawBalance(amountVal);
                _transactionManager.InsertTransaction(transaction);
                _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);


                if (ServiceFeeRequired(account.AccountNumber))
                {
                    var serviceCharge = new Transaction()
                    {
                        TransactionType = ((char)ConstValues.TransactionType.ServiceCharge),
                        AccountNumber = account.AccountNumber,
                        DestinationAccountNumber = null,
                        Amount = ConstValues.WithdrawFee,
                        Comment = null,
                        TransactionTimeUtc = DateTime.Today
                    };

                    accountNewBalance = accountNewBalance.ComputeWithdrawBalance(ConstValues.WithdrawFee);
                    _transactionManager.InsertTransaction(serviceCharge);
                    _accountManager.UpdateBalance(account.AccountNumber, accountNewBalance);
                }

                Console.WriteLine($"Withdraw of {amount:C} successful, account balance now {accountNewBalance:C}");
            }
        }
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
            if (amount > TransactionMath.ComputeAvailableBalance(account))
            {
                MiscUtils.PrintErrMsg("Ammount cannot be greater than available balance");
                return null;
            }

        return amount;
    }



    private void PrintMenu()
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