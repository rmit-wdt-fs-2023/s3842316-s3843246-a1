namespace MCBA.Model.DTO;

public class Customer
{
    public int CustomerID { get; init; }
    public string Name { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public int? PostCode { get; init; }
    public List<Account> Accounts { get; init; }
}

public class Account
{
    public int AccountNumber { get; init; }
    public char AccountType { get; init; }
    public int CustomerID { get; init; }
    public List<Transaction> Transactions { get; init; }
}

public class Transaction
{
    public double Amount { get; init; }
    public string? Comment { get; init; }
    public DateTime TransactionTimeUtc { get; init; }
}

public class Credential
{
    public int LoginID { get; init; }
    public string PasswordHash { get; init; }
}