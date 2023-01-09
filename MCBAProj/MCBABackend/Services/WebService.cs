using Newtonsoft.Json;
using MCBA.Model;
using MCBA.Managers;
namespace MCBA.Web.Service;

public static class WebService
{
	private static readonly string _url =
		"https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

	// Retreves Customers from Web API and add's them to the database
	public static void FetchAndPostWebCustomers(CustomerManager customerManager, AccountManager accountManager,
		TransactionManager transactionManager, CredentialManager credentialManager)
	{
        if (customerManager.Exists())
			return;

		using var client = new HttpClient();
		var json = client.GetStringAsync(_url).Result;

		var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
		{
			DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
		});

        // Checks if list is not null 
        if (customers.Any())
		{
			// Inserting data in DataBase
            foreach (var customer in customers)
            {
				customerManager.InsertCustomer(customer);
				foreach (var account in customer.Accounts)
				{
					accountManager.InsertAccount(account);
					foreach (var transaction in account.Transactions)
					{
						transactionManager.InsertTransaction(transaction);
					}
                }
				credentialManager.InsertCredential(customer.Login);
            }
        }

		// Debug - Delete later
		Console.WriteLine("Populated DB - DEBUG");
	}
}