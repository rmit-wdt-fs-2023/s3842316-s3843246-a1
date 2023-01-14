using System;
namespace MCBA.Managers
{
	public class DBManagerFactory
	{
		public CredentialManager _credentialManager { get; }
        public CustomerManager _customerManager { get; }
        public AccountManager _accountManager { get; }
        public TransactionManager _transactionManager { get; }

        public DBManagerFactory(string connection)
		{
			_credentialManager = new CredentialManager(connection);
			_customerManager = new CustomerManager(connection);
			_accountManager = new AccountManager(connection);
			_transactionManager = new TransactionManager(connection);
		}
	}
}

