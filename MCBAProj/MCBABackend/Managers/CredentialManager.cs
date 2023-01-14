using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
using MCBA.Utils;
namespace MCBA.Managers;

public class CredentialManager : AbstractDBManager
{
    public CredentialManager(string connection) : base(connection) { }

    // Inserts new Login details in databse
    public void InsertCredential(Credential credential)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText =
            @"INSERT INTO dbo.[Login] (LoginID, CustomerID, PasswordHash)" +
        "VALUES (@loginID, @customerID, @passwordHash)";
        cmd.Parameters.AddWithValue("loginID", credential.LoginID);
        cmd.Parameters.AddWithValue("customerID", credential.CustomerID);
        cmd.Parameters.AddWithValue("passwordHash", credential.PasswordHash);

        cmd.ExecuteNonQuery();
    }

    // Returns login details of the loginId
    public Credential GetCredentials(int loginId)
    {
        using var connection = new SqlConnection(ConnectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText = "SELECT * FROM dbo.[Login] WHERE LoginID = @loginId";
        cmd.Parameters.AddWithValue(nameof(loginId), loginId);

        var row = cmd.GetDataTable().Select().SingleOrDefault();

        return row != null ? ToCredential(row) : null;
    }

    // Converts datarow to credential object
    private Credential ToCredential(DataRow row)
    {
        var credential = new Credential()
        {
            LoginID = int.Parse(row.Field<string>(nameof(Credential.LoginID))),
            CustomerID = (int)row.Field<int>(nameof(Credential.CustomerID)),
            PasswordHash = (string)row.Field<string>(nameof(Credential.PasswordHash))
        };
        return credential;
    }
}

