using System.Data;
using Microsoft.Data.SqlClient;
namespace MCBA.Connnection.DB;

public class DBConnection
{

    private const string ConnectionStr =
        "server=rmit.australiaeast.cloudapp.azure.com;Encrypt=False;uid=s3842316_a1;database=s3842316_a1;pwd=abc123";


    public DBConnection()
	{
        DisconnectedAccess(ConnectionStr);
    }

    // Id decide to usde custom connection str
    public DBConnection(string connectionStr)
    {
        DisconnectedAccess(ConnectionStr);
    }

    private void DisconnectedAccess(string connectionStr)
    {
        using var connection = new SqlConnection(connectionStr);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "select * from dbo.Account";

        var table = new DataTable();
        new SqlDataAdapter(command).Fill(table);

        foreach (var x in table.Select())
        {
            Console.WriteLine($"{x["AccountNumber"]}\n{x["AccountType"]}\n{x["CustomerID"]}\n{x["Balance"]}\n");
        }

    }
}