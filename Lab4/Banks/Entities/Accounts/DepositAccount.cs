using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities.Accounts;

public class DepositAccount : IAccount
{
    private const decimal InitialAccountBalance = 0;
    private const decimal MinInterest = 0;
    private const decimal MinNonNegativeNumber = 0;
    private const int DaysInYear = 365;
    private const int MonthsInYear = 12;
    private const int NumberOfPercentagesInOneWhole = 100;

    public DepositAccount(Client owner, decimal cumulativeInterest, DateTime end, int id, DateTime openDate)
    {
        if (id <= MinNonNegativeNumber)
            throw new AccountException("Id cannot be negative");
        if (owner is null)
            throw new ArgumentException("Owner cannot be null");
        if (CumulativeInterest < MinInterest)
            throw new AccountException("Interest on balance must be non-negative number");

        Type = "Deposit";
        Id = id;
        OpenDate = openDate;
        Balance = InitialAccountBalance;
        Owner = owner;
        CumulativeInterest = cumulativeInterest;
        End = end;
    }

    public int Id { get; }
    public string Type { get; }
    public DateTime OpenDate { get; }
    public decimal Balance { get; private set; }
    public Client Owner { get; }

    public decimal WithdrawingLimit
    {
        get
        {
            if (CustomDateTime.Now.CompareTo(End) < 0)
                return 0;
            return Balance;
        }
    }

    public decimal CumulativeInterest { get; }
    public DateTime End { get; }

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
        if (CustomDateTime.Now.CompareTo(End) < 0)
            throw new AccountException("You cannot transfer money from the deposit account before it ends");

        Withdrawing(amount);
        anotherAccount.Replenishment(amount);
    }

    public void Withdrawing(decimal amount)
    {
        if (CustomDateTime.Now.CompareTo(End) < 0)
            throw new AccountException("You cannot transfer money from the deposit account before it ends");
        if (amount <= MinNonNegativeNumber)
            throw new AccountException("Withdrawing can only be for a negative amount");
        Balance -= amount;
    }

    public void Recalculate()
    {
        if (End.CompareTo(CustomDateTime.Now) < 0 || !CustomDateTime.Now.Day.Equals(OpenDate.Day)) return;
        Balance += Balance * CumulativeInterest / NumberOfPercentagesInOneWhole / MonthsInYear;
    }

    public string Info()
    {
        return $"Owner               : {Owner.FirstName} {Owner.SecondName}\n" +
               $"Type                : {Type}\n" +
               $"Balance             : {Balance}\n" +
               $"Cumulative interest : {CumulativeInterest}\n" +
               $"Open date           : {OpenDate}\n" +
               $"Closing date        : {End}\n" +
               $"Id                  : {Id}";
    }
}