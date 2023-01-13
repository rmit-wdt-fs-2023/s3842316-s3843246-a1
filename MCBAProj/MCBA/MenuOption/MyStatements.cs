using MCBA.Managers;
using MCBA.Menu.Options;
using MCBA.Model;
using static MCBA.Utils.MiscUtils;
using static MCBA.Utils.ConstValues;
using static MCBA.Utils.TransactionMath;
namespace MCBA.Menu.Option;

public class MyStatements
{

    private readonly AccountManager _accountManager;
    private Customer _customer;
    private List<Account> _accounts;
    private Account _account;

    public MyStatements(AccountManager accountManager, Customer customer)
    {
        _accountManager = accountManager;
        _customer = customer;
        _accounts = _customer.Accounts;

    }

    public void Run()
    {
        Console.WriteLine();
        PrintMenu();
        var usrInput = Console.ReadLine();
        if (int.TryParse(usrInput, out var option) && option.IsInRange(1, _accounts.Capacity))
        {
            _account = _accounts[--option];
            PrintStatement();
        }
    }

    private void PrintStatement()
    {
        var sortedTransaction = Sort();
        var pageNumber = 1;
        var pageSize = MaxTransactionPerStatmentPage;

        PrintAccountInfo();
        PrintNext(sortedTransaction, pageNumber);

        var exit = false;
        while (!exit)
        {
            Console.Write("\nEnter a option: ");
            var usrInput = Console.ReadLine();
            if (!char.TryParse(usrInput, out var option))
            {
                PrintErrMsg("Not an Valid Input");
                PrintNext(sortedTransaction, pageNumber);
                continue;
            }

            switch (option)
            {
                case 'n':
                    if (pageNumber != ((sortedTransaction.Count() + pageSize - 1) / pageSize))
                    {
                        PrintAccountInfo();
                        PrintNext(sortedTransaction, ++pageNumber);
                    }
                    else
                    {
                        PrintErrMsg("Last page reached");
                        PrintAccountInfo();
                        PrintNext(sortedTransaction, pageNumber);
                    }
                    break;
                case 'p':
                    if (pageNumber != 1)
                    {
                        PrintAccountInfo();
                        Console.WriteLine();
                        PrintNext(sortedTransaction, --pageNumber);
                    }
                    else
                    {
                        PrintErrMsg("First page reached");
                        PrintAccountInfo();
                        PrintNext(sortedTransaction, pageNumber);
                    }
                    break;
                case 'q':
                    exit = true;
                    break;
                default:
                    PrintErrMsg("Not a option");
                    PrintAccountInfo();
                    PrintNext(sortedTransaction, pageNumber);
                    break;
            }
        }
    }

    private void PrintNext(List<Transaction> transaction, int pageNumber)
    {
        var pageSize = MaxTransactionPerStatmentPage;
        var list = transaction.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        //Console.WriteLine(@"ID.20 {Type,20} {Account Number,20} {Destination,20} {Amount,20} {Time,20} {Comment,20}");
        Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-15} {4,-15} {5,-25} {6}",
            "ID", "Type", "Account Number", "Destination", "Amount", "Time", "Comment");


        for (var i = 0; i < list.Count(); i++)
        {
            var t = list[i];
            Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-15} {4,-15:C} {5,-25} {6}",
            t.TransactionID, t.TransactionType, t.AccountNumber,
            t.DestinationAccountNumber != null ? t.DestinationAccountNumber : "n/a",
            t.Amount, t.TransactionTimeUtc.ToString("dd/MM/yyy hh:mm tt"), t.Comment);
        }

        Console.Write(
            $"""
            Page {pageNumber} of {(transaction.Count() + pageSize - 1) / pageSize}

            Option: n(next page) | p (previous page) | q (quit)
            """);
    }

    private List<Transaction> Sort() => _account.Transactions.OrderByDescending(x => x.TransactionTimeUtc).ToList();

    private void PrintAccountInfo()
    {
        Console.WriteLine(
            $"\nSaving {_account.AccountNumber}, Balance {_account.Balance}, " +
            $"Available Balance {ComputeAvailableBalance(_account)}\n");
    }

    private void PrintMenu()
    {
        Console.WriteLine("--- MyStatement ---");
        for (var i = 0; i < _customer.Accounts.Capacity; i++)
        {
            Console.WriteLine($"{i + 1}. {_accounts[i].AccountType.GetAccStrFromChar()}\t" +
                $"{_accounts[i].AccountNumber}\t{_accounts[i].Balance:C}");
        }
        Console.Write("\nSelect an account: ");
    }
}