using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
namespace MCBA.Managers;

public class AccountManager
{
    private readonly string _connectionStr;

    public AccountManager(string connectionStr)
    {
        _connectionStr = connectionStr;
    }

    public void InsertAccount(Account account)
    {
        using var connection = new SqlConnection(_connectionStr);
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
}
