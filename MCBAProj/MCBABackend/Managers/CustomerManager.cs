using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
using MCBA.Utils;
using System.Net;

namespace MCBA.Managers;

public class CustomerManager
{
    private readonly string _connectionStr;

    public CustomerManager(string connectionStr)
	{
        _connectionStr = connectionStr;
	}

    public bool Exists()
    {
        using var connection = new SqlConnection(_connectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "select count(*) from dbo.Customer";

        var count = (int)cmd.ExecuteScalar();

        return count > 0;
    }

    public void InsertCustomer(Customer customer)
    {
        using var connection = new SqlConnection(_connectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText =
            @"INSERT INTO dbo.[Customer] (CustomerID, Name, Address, City, Postcode)" +
            "VALUES (@customerId, @name, @address, @city, @postcode)";
        cmd.Parameters.AddWithValue("customerId", customer.CustomerID);
        cmd.Parameters.AddWithValue("name", customer.Name);

        cmd.Parameters.AddWithValue("address", customer.Address.GetObjOrDbNull());
        cmd.Parameters.AddWithValue("city", customer.City.GetObjOrDbNull());
        cmd.Parameters.AddWithValue("postcode", customer.PostCode.GetObjOrDbNull());

        cmd.ExecuteNonQuery();
    }

    public Customer GetCustomer(Credential credential)
    {
        using var connection = new SqlConnection(_connectionStr);
        connection.Open();

        using var cmd = connection.CreateCommand();

        cmd.CommandText = "SELECT * FROM dbo.[Customer] WHERE CustomerID = @customerId";
        cmd.Parameters.AddWithValue("customerId", credential.CustomerID);

        var row = cmd.GetDataTable().Select().SingleOrDefault();

        return row != null ? ToCustomer(row) : null;
    }

    private Customer ToCustomer(DataRow row)
    {
        var accountManager = new AccountManager(_connectionStr);
        var credentialManager = new CredentialManager(_connectionStr);
        var customer = new Customer()
        {
            CustomerID = row.Field<int>(nameof(Customer.CustomerID)),
            Name = row.Field<string>(nameof(Customer.Name)),
            Address = row.Field<string?>(nameof(Customer.Address)),
            City = row.Field<string?>(nameof(Customer.City)),
            PostCode = int.Parse(row.Field<string?>(nameof(Customer.PostCode))),
            Accounts = accountManager.GetAccounts(row.Field<int>(nameof(Customer.CustomerID))),
            Login = credentialManager.GetCredentials(row.Field<int>(nameof(Customer.CustomerID)))
        };
        return customer;
    }
}
