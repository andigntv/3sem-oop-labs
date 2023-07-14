using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities.Accounts;

public class DebitAccount : IAccount
{
    private const decimal InitialAccountBalance = 0;
    private const decimal MinInterestOnBalance = 0;
    private const decimal MinNonNegativeNumber = 0;
    private const int DaysInYear = 365;
    private const int NumberOfPercentagesInOneWhole = 100;
    private decimal _accumulatedBalance;
    public DebitAccount(Client owner, decimal interestOnBalance, int id, DateTime openDate)
    {
        if (id <= MinNonNegativeNumber)
            throw new AccountException("Id cannot be negative");
        if (owner is null)
            throw new ArgumentException("Owner cannot be null");
        if (interestOnBalance < MinInterestOnBalance)
            throw new AccountException("Interest on balance must be non-negative number");

        Type = "Debit";
        Id = id;
        OpenDate = openDate;
        Balance = InitialAccountBalance;
        Owner = owner;
        InterestOnBalance = interestOnBalance;
        _accumulatedBalance = MinNonNegativeNumber;
    }

    public int Id { get; }
    public string Type { get; }
    public DateTime OpenDate { get; }
    public decimal Balance { get; private set; }
    public decimal WithdrawingLimit => Balance;
    public Client Owner { get; }
    public decimal InterestOnBalance { get; private set; }

    public void Replenishment(decimal amount)
    {
        if (amount <= MinNonNegativeNumber)
            throw new AccountException("Replenishment can only be for a positive amount");
        Balance += amount;
    }

    public void Transfer(decimal amount, IAccount anotherAccount)
    {
        if (anotherAccount is null)
            throw new ArgumentException("Account cannot be null");
        Withdrawing(amount);
        anotherAccount.Replenishment(amount);
    }

    public void Withdrawing(decimal amount)
    {
        if (amount <= MinNonNegativeNumber)
            throw new AccountException("Withdrawing can only be for a negative amount");
        Balance -= amount;
    }

    public void ChangeInterestOnBalance(decimal value)
    {
        if (value < MinInterestOnBalance)
            throw new AccountException("Interest on balance must be non-negative number");
        InterestOnBalance = value;
    }

    public void Recalculate()
    {
        _accumulatedBalance += Balance * InterestOnBalance / NumberOfPercentagesInOneWhole / DaysInYear;
        if (!CustomDateTime.Now.Day.Equals(OpenDate.Day)) return;
        Balance += _accumulatedBalance;
        _accumulatedBalance = 0;
    }

    public string Info()
    {
        return $"Owner               : {Owner.FirstName} {Owner.SecondName}\n" +
               $"Type                : {Type}\n" +
               $"Balance             : {Balance}\n" +
               $"Interest on balance : {InterestOnBalance}\n" +
               $"Open date           : {OpenDate}\n" +
               $"Id                  : {Id}";
    }
}