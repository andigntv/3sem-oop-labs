using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities.Transactions;

public class TransferTransaction : ITransaction
{
    private const decimal MinNonNegativeNumber = 0;
    public TransferTransaction(IAccount account, DateTime dateTime, int id, decimal amount, IAccount recipientAccount)
    {
        ArgumentNullException.ThrowIfNull(account);
        if (id < MinNonNegativeNumber)
            throw new TransactionException("Transaction's id cannot be negative");
        if (amount < MinNonNegativeNumber)
            throw new TransactionException("Transaction's amount cannot be negative");
        ArgumentNullException.ThrowIfNull(recipientAccount);

        Type = "Transfer";
        Account = account;
        DateTime = dateTime;
        Id = id;
        Amount = amount;
        RecipientAccount = recipientAccount;
    }

    public int Id { get; }

    public string Type { get; }
    public IAccount Account { get; }
    public IAccount RecipientAccount { get; }
    public DateTime DateTime { get; }
    public decimal Amount { get; }
    public void Cancel()
    {
        RecipientAccount.Transfer(Amount, Account);
    }
}