using MCBA.Model;

namespace MCBA.Utils;
internal static class TransactionMath
{
    internal static decimal ComputeAvailableBalance(Account account)
	{
        if (account.AccountType == ((char)ConstValues.AccountType.Saving))
            return account.Balance;
        else
            return (account.Balance - ConstValues.CheckingAccountMinBalance) <=
                0 ? 0 : (account.Balance - ConstValues.CheckingAccountMinBalance);
    }

    internal static decimal ComputeWithdrawBalance(this decimal accountBalance, decimal amount) => (accountBalance - amount);
    internal static decimal ComputeDepositBalance(this decimal accountBalance, decimal amount) => (accountBalance + amount);

}

