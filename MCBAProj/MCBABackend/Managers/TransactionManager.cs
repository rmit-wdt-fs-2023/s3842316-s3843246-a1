using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
using MCBA.Utils;
namespace MCBA.Managers;

public class TransactionManager : AbstractDBManager
{
    public TransactionManager(string connection) : base(connection) { }

    // Inserts new transaction in databse
    public void InsertTransaction(Transaction transaction)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            @"INSERT INTO dbo.[Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, " +
            "Comment, TransactionTimeUtc)" +
            "VALUES (@transactionType, @accountNumber, @destinationAccountNumber, @amount, " +
            "@comment, @transactionTimeUtc)";

        cmd.Parameters.AddWithValue("transactionType", transaction.TransactionType);
        cmd.Parameters.AddWithValue("accountNumber", transaction.AccountNumber);

        cmd.Parameters.AddWithValue("destinationAccountNumber", transaction.DestinationAccountNumber.GetObjOrDbNull());

        cmd.Parameters.AddWithValue("amount", transaction.Amount);

        cmd.Parameters.AddWithValue("comment", transaction.Comment.GetObjOrDbNull());

        cmd.Parameters.AddWithValue("transactionTimeUtc", transaction.TransactionTimeUtc);

        cmd.ExecuteNonQuery();
    }

    // Returns list of all transactions of specific accountNumber
    public List<Transaction> GetTransactions(int accountNumber)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText = "SELECT * FROM dbo.[Transaction] WHERE AccountNumber = @accountNumber";
        cmd.Parameters.AddWithValue(nameof(accountNumber), accountNumber);

        return cmd.GetDataTable().Select().Select(x => new Transaction
        {
            TransactionID = x.Field<int>(nameof(Transaction.TransactionID)),
            TransactionType = char.Parse(x.Field<string>(nameof(Transaction.TransactionType))),
            AccountNumber = x.Field<int>(nameof(Transaction.AccountNumber)),
            DestinationAccountNumber = x.Field<int?>(nameof(Transaction.DestinationAccountNumber)),
            Amount = x.Field<decimal>(nameof(Transaction.Amount)),
            Comment = x.Field<string?>(nameof(Transaction.Comment)),
            TransactionTimeUtc = x.Field<DateTime>(nameof(Transaction.TransactionTimeUtc))

        }).ToList();
    }
}
