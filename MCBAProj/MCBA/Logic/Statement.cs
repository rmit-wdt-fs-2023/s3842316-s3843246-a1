using System;
using MCBA.Model;
namespace MCBA.SLogic
{
	
	public class Statement
	{
        private Customer customer;
        private Account Saccount;
        private Account Caccount;
        private Transaction Stransaction;
        private Transaction Ctransaction;

        public Statement(Customer customer)
		{
			this.customer = customer;
		}

		public void PrintStatement()
		{
			int check = 0;
			int savings = 0;


			for (int i = 0; i < customer.Accounts.Count; i++)
			{
				var ac = customer.Accounts[i];
				if (ac.AccountType == 'S')
				{
					Console.WriteLine((i + 1) + ". Savings -   " + ac.AccountNumber + "  $" + ac.Balance);
					savings = i + 1;
					Saccount = ac;

				}
				else if (ac.AccountType == 'C')
				{
					Console.WriteLine((i + 1) + ". Checking -    " + ac.AccountNumber + "  $" + ac.Balance);
					check = i + 1;
					Caccount = ac;
				}
			}

			Console.WriteLine("Select an account: ");
			var choice = int.Parse(Console.ReadLine());
			
				


			if ((choice == 1 && savings == 1) || (choice == 2 && savings == 2))
			{
				int pages = Saccount.Transactions.Count;
				int index = 4;
                  
						
						Console.WriteLine("Savings " + Saccount.AccountNumber + ", Balance: $" + Saccount.Balance + ", Available Balance: $" + Saccount.Balance);

						Console.WriteLine("| {0,-10}  {1,-20}  {2,-20}  {3,-20}  {4,-20}  {5,-15}  {6,-20}  {7,-20}", "ID", "Type", "Account Number", "Destination", "Amount", "Time", "Comment");
						Console.WriteLine("| {0,-10}  {1,-20}  {2,-20}  {3,-20}  {4,-20}  {5,-15}  {6,-20}  {7,-20}", 1, Saccount.Transactions[index-3].TransactionType, Saccount.Transactions[index - 3].AccountNumber, Saccount.Transactions[index - 3].DestinationAccountNumber, Saccount.Transactions[index - 3].Amount, Saccount.Transactions[index - 3].TransactionTimeUtc, Saccount.Transactions[index - 3].Comment); 
						Console.WriteLine("| {0,-10}  {1,-20}  {2,-20}  {3,-20}  {4,-20}  {5,-15}  {6,-20}  {7,-20}", 2, Saccount.Transactions[index - 2].TransactionType, Saccount.Transactions[index - 2].AccountNumber, Saccount.Transactions[index - 2].DestinationAccountNumber, Saccount.Transactions[index - 2].Amount, Saccount.Transactions[index - 2].TransactionTimeUtc, Saccount.Transactions[index - 2].Comment);
						Console.WriteLine("| {0,-10}  {1,-20}  {2,-20}  {3,-20}  {4,-20}  {5,-15}  {6,-20}  {7,-20}", 3, Saccount.Transactions[index - 1].TransactionType, Saccount.Transactions[index - 1].AccountNumber, Saccount.Transactions[index - 1].DestinationAccountNumber, Saccount.Transactions[index - 1].Amount, Saccount.Transactions[index - 1].TransactionTimeUtc, Saccount.Transactions[index - 1].Comment);
						Console.WriteLine("| {0,-10}  {1,-20}  {2,-20}  {3,-20}  {4,-20}  {5,-15}  {6,-20}  {7,-20}", 4, Saccount.Transactions[index].TransactionType, Saccount.Transactions[index].AccountNumber, Saccount.Transactions[index].DestinationAccountNumber, Saccount.Transactions[index].Amount, Saccount.Transactions[index].TransactionTimeUtc, Saccount.Transactions[index].Comment);
						Console.WriteLine();
					
                
     //               foreach(Transaction tra in Enumerable.Range()
					//{

					//}
				
					
				
			}
			else if ((choice == 1 && check == 1) || (choice == 2 && check == 2))
			{
				Console.WriteLine("Savings " + Caccount.AccountNumber + ", Balance: $" + Caccount.Balance + ", Available Balance: $" + Caccount.Balance);
			}
			

            
        }


			
	}
}

