//using System.Data;
//using Microsoft.Data.SqlClient;
//using MCBA.Model.DTO;
//namespace MCBA.Connection.DB;

//public class DBConnection
//{

//    private const string ConnectionStr =
//        "server=rmit.australiaeast.cloudapp.azure.com;Encrypt=False;uid=s3842316_a1;database=s3842316_a1;pwd=abc123";


//    public DBConnection()
//    { }

//    public void DisconnectedAccess()
//    {
//        using var connection = new SqlConnection(ConnectionStr);
//        connection.Open();

//        var command = connection.CreateCommand();
//        command.CommandText = "select * from dbo.Customer";

//        var table = new DataTable();
//        new SqlDataAdapter(command).Fill(table);

//        foreach (var x in table.Select())
//        {
//            Console.WriteLine($"{x["CustomerID"]}\n{x["Name"]}\n{x["Address"]}\n{x["City"]}\n{x["Postcode"]}\n");
//        }

//    }

//    public void InsertCustomers(List<Customer> customers)
//    {
//        using var connection = new SqlConnection(ConnectionStr);
//        connection.Open();
//        Console.WriteLine("State: {0}", connection.State);

//        var cmd = connection.CreateCommand();
//        cmd.CommandText =
//                "INSERT INTO dbo.Customer (CustomerID, Name, Address, City, Postcode)" +
//                "VALUES (222, TEstName, TestAdd, TestCity, 2000)";

//        //foreach (var customer in customers)
//        //{
//        //    cmd.CommandText =
//        @"INSERT INTO dbo.Customer (CustomerID, Name, Address, City, Postcode)" +
//        "VALUES (@CustomerID, @Name, @Address, @City, @Postcode)";
//        //    cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
//        //    cmd.Parameters.AddWithValue("@Name", customer.Name);
//        //    cmd.Parameters.AddWithValue("@Address", customer.Address);
//        //    cmd.Parameters.AddWithValue("@City", customer.City);
//        //    cmd.Parameters.AddWithValue("@Postcode", customer.PostCode);
//        //}

//        Console.WriteLine("HERE1");
//        var updates = cmd.ExecuteNonQuery();
//        Console.WriteLine("HERE2");

//        Console.WriteLine($"{updates} rows updated.\n");
//    }
//}