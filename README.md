# s3842316-s3843246-a1

### F) Design Patterns
 1. Dependency Injection
	 - Used - Files 'MCBA/Program.cs' -> 'MCBABackend/Managers/DBManagerFactory',  'MCBA/Impl/Menu.cs' -> 'MCBA/MenuOptions/AbstractTransactions.cs', 'MCBA/MenuOptions/MyStatements.cs' 
	- Where - 'Program.cs'
		- 'Program.cs' Constructor Injector - 
			>  var dbManagerFactory = new DBManagerFactory(connectionStr);				
			>  WebService.FetchAndPostWebCustomers(dbManagerFactory);
			>             new Menu(dbManagerFactory).Run();
		 - 'Menu.cs ' Constructor Injector - 
			 > var login = new LoginData(_manager);
			 > new Deposit(_manager, _customer).Run();
			 > new Withdraw(_manager, _customer).Run();
			 > new Transfer(_manager, _customer).Run();
			 > new MyStatements(_manager, _customer).Run();
	- Short Summary -
		>Design pattern in which an object receives other objects that it depends on.
	- Advantage in Proj
		> Reduced code/code duplication
		

		 > Without using injection new instance of required class will be required in all corresponding classes, If code is changed or new class is added or required all constructs will need to be updated 
		 
		> By using this code is reduce and if in future connection string is changed OR new class is added in Factory the constructors won't need to be changed.

2. Factory
	 - Used - Files 'MCBABackend/Managers/DBManagerFactory', Program.cs
	- Where - 'Program.cs'
		- 'Program.cs'  - 
			>  var dbManagerFactory = new DBManagerFactory(connectionStr);				
		 - 'DBManagerFactory ' 
	- Short Summary -
		> We can create the object without exposing the creation logic to the client and refer to newly created object using a common interface even tho an interface is not an integral part of this pattern. In my project interface is not used instead accusers are used and all obj are constructed with the constructor
		
	- Advantage in Proj
		> Reduced code/code duplication
		
		> No multiple instances of object
		
	- Without using injection code would look like this - 
	
	       var customerManager = new CustomerManager(connectionStr); 
	       var accountManager = new AccountManager(connectionStr); 
	       var transactionManager = new TransactionManager(connectionStr);
	       var credentialManager = new CredentialManager(connectionStr);
	       
	       // Populationg Database if empty
	       WebService.FetchAndPostWebCustomers(customerManager,
	       accountManager, transactionManager, credentialManager);
	       
	       // Runs the main system new Menu(credentialManager,
	       customerManager, accountManager, transactionManager).Run();





			
			 
