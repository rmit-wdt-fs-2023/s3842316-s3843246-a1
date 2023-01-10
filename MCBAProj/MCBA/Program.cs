using MCBAConnection;
using MCBA.Managers;
using MCBA.Web.Service;

public class Program
{
    private static void Main()
    {
        var connectionStr = Connection.GetConnection();
        if (connectionStr != null)
        {
            var customerManager = new CustomerManager(connectionStr);
            var accountManager = new AccountManager(connectionStr);
            var transactionManager = new TransactionManager(connectionStr);
            var credentialManager = new CredentialManager(connectionStr);

            // Populationg Database if empty
            WebService.FetchAndPostWebCustomers(customerManager, accountManager,
                transactionManager, credentialManager);

        }
        else
        {
            Console.WriteLine("Error: Database could not be loaded");
        }

    }
}