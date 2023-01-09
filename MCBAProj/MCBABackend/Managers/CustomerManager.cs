using System.Data;
using Microsoft.Data.SqlClient;
using MCBA.Model;
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
            @"INSERT INTO dbo.Customer (CustomerID, Name, Address, City, Postcode)" +
        "VALUES (@customerId, @name, @address, @city, @postcode)";
        cmd.Parameters.AddWithValue("customerId", customer.CustomerID);
        cmd.Parameters.AddWithValue("name", customer.Name);
        cmd.Parameters.AddWithValue("address", customer.Address);
        cmd.Parameters.AddWithValue("city", customer.City);
        cmd.Parameters.AddWithValue("postcode", customer.PostCode);

        cmd.ExecuteNonQuery();
    }
}
