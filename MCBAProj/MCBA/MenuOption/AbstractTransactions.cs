using MCBA.Managers;
using MCBA.Model;
using MCBA.Utils;
namespace MCBA.Menu.Option;

public abstract class AbstractTransactions
{
    protected readonly AccountManager _accountManager;
    protected readonly TransactionManager _transactionManager;
    protected Customer _customer;
    protected List<Account> _accounts;

    public AbstractTransactions(AccountManager accountManager,
        TransactionManager transactionManager, Customer customer)
    {
        _accountManager = accountManager;
        _transactionManager = transactionManager;
        _customer = customer;
        _accounts = _customer.Accounts;
    }

    public abstract void Run();

    protected string GetCommentInput()
    {
        Console.Write("Enter comment (n to quit, max length 30: ");
        string comment = string.Empty;
        var keyInfo = Console.ReadKey(true);
        var keyRead = keyInfo.Key;

        if (keyRead == ConsoleKey.N)
        {
            // Checks is key like shift is pressed so no uppercase
            if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.WriteLine();
                comment = null;
            }
        }
        else if (comment == string.Empty)
        {
            Console.Write(keyInfo.KeyChar);
            comment = keyRead.ToString() + Console.ReadLine();
        }
        return comment;
    }

    protected bool ServiceFeeRequired(int accountNo)
    {
        int noOfTransactions = GetNoOfTransactions(accountNo);
        return noOfTransactions > ConstValues.MaxFreeTransactionsAllowed ? true : false;
    }

    protected int GetNoOfTransactions(int accountNo)
    {
        List<Transaction> transactions = _transactionManager.GetTransactions(accountNo);

        int noOfTransactions = 0;
        foreach (var transaction in transactions)
        {
            if (transaction.TransactionType == ((char)ConstValues.TransactionType.Withdraw)
                || transaction.TransactionType == ((char)ConstValues.TransactionType.Transfer))
                noOfTransactions++;
        }
        return noOfTransactions;
    }
}