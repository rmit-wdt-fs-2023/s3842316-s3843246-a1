using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
namespace MCBA.Managers;

public class TransactionManager
{
    private readonly string _connectionStr;

    public TransactionManager(string connectionStr)
    {
        _connectionStr = connectionStr;
    }

    public void InsertTransaction(Transaction transaction)
    {
        using var connection = new SqlConnection(_connectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText =
            @"INSERT INTO dbo.Transaction (TransactionType, AccountNumber, " +
        "DestinationAccountNumber, Amount, Comment, TransactionTimeUtc)" +
        "VALUES ( @transactionType, @accountNumber, @destinationAccountNumber, " +
        "@amount, @comment, @transactionTimeUtc)";

        cmd.Parameters.AddWithValue("transactionType", transaction.TransactionType);
        cmd.Parameters.AddWithValue("accountNumber", transaction.AccountNumber);
        cmd.Parameters.AddWithValue("destinationAccountNumber", transaction.DestinationAccountNumber);
        cmd.Parameters.AddWithValue("amount", transaction.Amount);
        cmd.Parameters.AddWithValue("comment", transaction.Comment);
        cmd.Parameters.AddWithValue("transactionTimeUtc", transaction.TransactionTimeUtc);

        cmd.ExecuteNonQuery();
    }
}
