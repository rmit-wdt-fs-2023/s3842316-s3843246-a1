using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
using MCBAConnection;
using MCBA.Utils;

namespace MCBA.Managers;

public class AccountManager : AbstractDBManager
{
    public AccountManager(string connection) : base(connection) { }

    // Returns all available accounts
    public List<Account> All()
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * from dbo.[Account]";

        var transactionManager = new TransactionManager(ConnectionStr);

        return cmd.GetDataTable().Select().Select(x => new Account
        {
            AccountNumber = x.Field<int>(nameof(Account.AccountNumber)),
            AccountType = char.Parse(x.Field<string>(nameof(Account.AccountType))),
            CustomerID = x.Field<int>(nameof(Account.CustomerID)),
            Balance = x.Field<decimal>(nameof(Account.Balance)),
            Transactions = transactionManager.GetTransactions(x.Field<int>(nameof(Account.AccountNumber)))

        }).ToList();
    }

    // Insert Account into database
    public void InsertAccount(Account account)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText =
            @"INSERT INTO dbo.[Account] (AccountNumber, AccountType, CustomerID, Balance)" +
        "VALUES (@accountNumber, @accountType, @customerID, @balance)";
        cmd.Parameters.AddWithValue("accountNumber", account.AccountNumber);
        cmd.Parameters.AddWithValue("accountType", account.AccountType);
        cmd.Parameters.AddWithValue("customerID", account.CustomerID);
        cmd.Parameters.AddWithValue("balance", account.Balance);

        cmd.ExecuteNonQuery();
    }

    // Returns list of customers accounts
    public List<Account> GetAccounts(int customerId)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText = "SELECT * FROM dbo.[Account] WHERE CustomerID = @customerId";
        cmd.Parameters.AddWithValue(nameof(customerId), customerId);

        var transactionManager = new TransactionManager(ConnectionStr);

        return cmd.GetDataTable().Select().Select(x => new Account
        {
            AccountNumber = x.Field<int>(nameof(Account.AccountNumber)),
            AccountType = char.Parse(x.Field<string>(nameof(Account.AccountType))),
            CustomerID = x.Field<int>(nameof(Account.CustomerID)),
            Balance = x.Field<decimal>(nameof(Account.Balance)),
            Transactions = transactionManager.GetTransactions(x.Field<int>(nameof(Account.AccountNumber)))

        }).ToList();
    }

    // Changes the cureent balance to newer balance
    public void UpdateBalance(int accountNumber, decimal balance)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText =
            "UPDATE dbo.[Account] SET Balance = @balance WHERE AccountNumber = @accountNumber";
        cmd.Parameters.AddWithValue("balance", balance);
        cmd.Parameters.AddWithValue("accountNumber", accountNumber);

        cmd.ExecuteNonQuery();
    }
}
