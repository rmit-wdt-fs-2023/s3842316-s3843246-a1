namespace MCBA.Model;

public class Customer
{
    public required int CustomerID { get; init; }
    public required string Name { get; init; }
    public required string? Address { get; init; }
    public required string? City { get; init; }
    public required int? PostCode { get; init; }
    public required List<Account> Accounts { get; init; }
    public required Credential Login { get; init; }
}

public class Account
{
    public required int AccountNumber { get; init; }
    public required char AccountType { get; init; }
    public required int CustomerID { get; set; }
    public required decimal Balance { get; set; }
    public required List<Transaction> Transactions { get; init; }
}

public class Transaction
{
    public int TransactionID { get; init; }
    public required char TransactionType { get; set; }
    public required int AccountNumber { get; set; }
    public required int? DestinationAccountNumber { get; init; }
    public required decimal Amount { get; init; }
    public string? Comment { get; init; }
    public required DateTime TransactionTimeUtc { get; init; }
}

public class Credential
{
    public required int LoginID { get; set; }
    public required int CustomerID { get; set; }
    public required string PasswordHash { get; set; }
}