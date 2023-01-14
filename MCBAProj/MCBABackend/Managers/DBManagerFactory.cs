using System;
namespace MCBA.Managers
{
	public class DBManagerFactory
	{
		private string _connectionStr;
		public CredentialManager _credentialManager { get; }
        public CustomerManager _customerManager { get; }
        public AccountManager _accountManager { get; }
        public TransactionManager _transactionManager { get; }

        public DBManagerFactory(string connection)
		{
			_connectionStr = connection;
			_credentialManager = new CredentialManager(_connectionStr);
			_customerManager = new CustomerManager(_connectionStr);
			_accountManager = new AccountManager(_connectionStr);
			_transactionManager = new TransactionManager(_connectionStr);
		}
	}
}

