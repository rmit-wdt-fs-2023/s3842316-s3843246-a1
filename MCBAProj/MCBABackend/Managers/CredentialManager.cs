using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
namespace MCBA.Managers;

public class CredentialManager
{
    private readonly string _connectionStr;

    public CredentialManager(string connectionStr)
    {
        _connectionStr = connectionStr;
    }

    public void InsertCredential(Credential credential)
    {
        using var connection = new SqlConnection(_connectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText =
            @"INSERT INTO dbo.Login (LoginID, CustomerID, PasswordHash)" +
        "VALUES (@loginID, @customerID, @passwordHash)";
        cmd.Parameters.AddWithValue("loginID", credential.LoginID);
        cmd.Parameters.AddWithValue("customerID", credential.CustomerID);
        cmd.Parameters.AddWithValue("passwordHash", credential.PasswordHash);

        cmd.ExecuteNonQuery();
    }
}
