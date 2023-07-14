using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities.Transactions;

public class WithdrawingTransaction : ITransaction
{
    private const decimal MinNonNegativeNumber = 0;
    public WithdrawingTransaction(IAccount account, DateTime dateTime, int id, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(account);
        if (id < MinNonNegativeNumber)
            throw new TransactionException("Transaction's id cannot be negative");
        if (amount < MinNonNegativeNumber)
            throw new TransactionException("Transaction's amount cannot be negative");

        Type = "Withdrawing";
        Account = account;
        DateTime = dateTime;
        Id = id;
        Amount = amount;
    }

    public int Id { get; }

    public string Type { get; }
    public IAccount Account { get; }
    public DateTime DateTime { get; }
    public decimal Amount { get; }
    public void Cancel()
    {
        Account.Replenishment(Amount);
    }
}