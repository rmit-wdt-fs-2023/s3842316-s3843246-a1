using Newtonsoft.Json;
using MCBA.Model;
using MCBA.Managers;
using MCBA.Utils;
namespace MCBA.Web.Service;

public static class WebService
{
	private static readonly string _url =
		"https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

	// Retreves Customers from Web API and add's them to the database
	public static void FetchAndPostWebCustomers(DBManagerFactory manager)
	{
        if (manager._customerManager.Exists())
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
                manager._customerManager.InsertCustomer(customer);
				foreach (var account in customer.Accounts)
				{
					account.CustomerID = customer.CustomerID;
					account.Balance = ComputeBalance(account);
                    manager._accountManager.InsertAccount(account);

					foreach (var transaction in account.Transactions)
					{
						transaction.TransactionType = MiscUtils.Deposit;
						transaction.AccountNumber = account.AccountNumber;
                        manager._transactionManager.InsertTransaction(transaction);
					}
				}
				customer.Login.CustomerID = customer.CustomerID;
                manager._credentialManager.InsertCredential(customer.Login);
			}
		}
	}

	// Calculates Balance through transactions
	private static decimal ComputeBalance(Account account)
	{
		decimal balance = 0;
		foreach (var transaction in account.Transactions)
		{
			balance += transaction.Amount;
		}
		return balance;
	}
}