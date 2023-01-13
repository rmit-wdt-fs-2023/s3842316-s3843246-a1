namespace MCBA.Utils;

internal static class ConstValues
{
	public enum AccountType
	{
		Saving = 'S',
		Checking = 'C'
	}

	public enum TransactionType
	{
		Deposit = 'D',
		Withdraw = 'W',
		Transfer = 'T',
		ServiceCharge = 'S'

	}

	public const decimal CreditAccountMinBalance = 300;

	public const int MaxCommentLenght = 30;
}

