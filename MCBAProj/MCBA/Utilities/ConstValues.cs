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

    public const decimal CheckingAccountMinBalance = 300;

	public const int MaxCommentLenght = 30;

	public const int MaxFreeTransactionsAllowed = 2;

	public const decimal WithdrawFee = 0.05M;

	public const decimal TransferFee = 0.10M;

	public const int MaxTransactionPerStatmentPage = 4;
}

