using Banks.Entities.Accounts;
using Banks.Entities.Transactions;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities;

public class Bank
{
    private const decimal MinNonNegativeNumber = 0;
    private List<IAccount> _accounts;
    private List<Client> _clients;
    private List<ITransaction> _transactions;
    private int _transactionId = 1;
    private int _accountId = 1;

    public Bank(string name, decimal creditLimit, decimal creditRate, decimal interestOnBalance, decimal cumulativeInterest)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Bank name cannot be empty");
        if (creditLimit < MinNonNegativeNumber)
            throw new ArgumentException("Credit limit must be non-negative number");
        if (creditRate < MinNonNegativeNumber)
            throw new ArgumentException("Credit rate must be non-negative number");
        if (interestOnBalance < MinNonNegativeNumber)
            throw new ArgumentException("Interest on balance must be non-negative number");
        if (cumulativeInterest < MinNonNegativeNumber)
            throw new ArgumentException("Cumulative interest must be non-negative number");

        Name = name;
        CreditLimit = creditLimit;
        CreditRate = creditRate;
        InterestOnBalance = interestOnBalance;
        CumulativeInterest = cumulativeInterest;
        _clients = new List<Client>();
        _accounts = new List<IAccount>();
        _transactions = new List<ITransaction>();
    }

    public string Name { get; }
    public IReadOnlyList<Client> Clients => _clients;
    public IReadOnlyList<IAccount> Accounts => _accounts;
    public IReadOnlyList<ITransaction> Transactions => _transactions;
    public decimal CreditLimit { get; private set; }
    public decimal CreditRate { get; private set; }
    public decimal InterestOnBalance { get; private set; }
    public decimal CumulativeInterest { get; private set; }

    public void AddClient(Client client)
    {
        if (client is null)
            throw new ArgumentException("Client cannot be null");
        _clients.Add(client);
    }

    public void AddAccountToClient(IAccount account, Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        client.AddAccount(account);
        _accounts.Add(account);
    }

    public CreditAccount CreateCreditAccount(Client client)
    {
        var result = new CreditAccount(client, CreditRate, CreditLimit, _accountId++, CustomDateTime.Now);
        _accounts.Add(result);
        client.AddAccount(result);
        return result;
    }

    public DebitAccount CreateDebitAccount(Client client)
    {
        var result = new DebitAccount(client, InterestOnBalance, _accountId++, CustomDateTime.Now);
        _accounts.Add(result);
        client.AddAccount(result);
        return result;
    }

    public DepositAccount CreateDepositAccount(Client client, DateTime end)
    {
        var result = new DepositAccount(client, CumulativeInterest, end, _accountId++, CustomDateTime.Now);
        _accounts.Add(result);
        client.AddAccount(result);
        return result;
    }

    public ReplenishmentTransaction Replenish(IAccount account, decimal amount)
    {
        if (account is null)
            throw new BankException("Account cannot be null");

        account.Replenishment(amount); // All validation in account method
        var result = new ReplenishmentTransaction(account, CustomDateTime.Now, _transactionId++, amount);
        _transactions.Add(result);
        return result;
    }

    public TransferTransaction Transfer(IAccount account, IAccount recipientAccount, decimal amount)
    {
        if (account is null || recipientAccount is null)
            throw new BankException("Account cannot be null");

        account.Transfer(amount, recipientAccount);
        var result = new TransferTransaction(account, CustomDateTime.Now, _transactionId++, amount, recipientAccount);
        _transactions.Add(result);
        return result;
    }

    public WithdrawingTransaction Withdraw(IAccount account, decimal amount)
    {
        if (account is null)
            throw new BankException("Account cannot be null");

        account.Withdrawing(amount);
        var result = new WithdrawingTransaction(account, CustomDateTime.Now, _transactionId++, amount);
        _transactions.Add(result);
        return result;
    }

    public void CancelTransaction(int id)
    {
        ITransaction? transaction = FindTransaction(id);
        if (transaction is null)
            throw new BankException("No such transaction");

        transaction.Cancel();
        _transactions.Remove(transaction);
    }

    public ITransaction? FindTransaction(int id)
    {
        return _transactions.Find(x => x.Id == id);
    }

    public void Update()
    {
        foreach (IAccount account in _accounts)
            account.Recalculate();
    }

    public void CloseAccount(int id)
    {
        IAccount? account = FindAccount(id);
        if (account is null)
            throw new BankException("No such account");

        account.Owner.DeleteAccount(id);
        _accounts.Remove(account);
    }

    public IAccount? FindAccount(int id)
    {
        return _accounts.Find(x => x.Id == id);
    }

    public Client? FindClient(int id)
    {
        return _clients.Find(x => x.Id == id);
    }

    public void NotifySubscribers(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty");
        foreach (Client client in _clients.Where(client => client.Subscribed))
            client.Notify(message);
    }
}