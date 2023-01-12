using MCBAConnection;
using MCBA.Managers;
using MCBA.Web.Service;
using MCBA.Impl.Run;

/*
 * Global - Some Method's were Adapted and modified accordinf to code
 *          from Rmit/Week3/InventoryPriceManagement||WebServiceAndDatabaseExample
 * Another Adapted methods have been commented above the methods
 */

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

            // Runs the main system
            new Menu(credentialManager, customerManager).Run();
        }
        else
        {
            Console.WriteLine("Fatal Err: Database could not be loaded");
        }

    }
}