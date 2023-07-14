using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities.Accounts;

public class CreditAccount : IAccount
{
    private const decimal InitialAccountBalance = 0;
    private const decimal MinAnnualInterestRate = 0;
    private const decimal MinCreditLimit = 0;
    private const decimal MinNonNegativeNumber = 0;
    private const int DaysInYear = 365;
    private const int NumberOfPercentagesInOneWhole = 100;

    public CreditAccount(Client owner, decimal annualInterestRate, decimal creditLimit, int id, DateTime openDate)
    {
        if (id <= MinNonNegativeNumber)
            throw new AccountException("Id cannot be negative");
        if (owner is null)
            throw new ArgumentException("Owner cannot be null");
        if (annualInterestRate < MinAnnualInterestRate)
            throw new AccountException("Annual Interest Rate must be non-negative number");
        if (creditLimit < MinCreditLimit)
            throw new AccountException("Credit limit must be non-negative number");

        Type = "Credit";
        Id = id;
        OpenDate = openDate;
        Owner = owner;
        Balance = InitialAccountBalance;
        AnnualInterestRate = annualInterestRate;
        CreditLimit = creditLimit;
    }

    public int Id { get; }
    public string Type { get; }
    public DateTime OpenDate { get; }
    public decimal Balance { get; private set; }

    public Client Owner { get; }
    public decimal AnnualInterestRate { get; private set; }
    public decimal CreditLimit { get; private set; }

    public decimal WithdrawingLimit => Balance + CreditLimit;

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
        if (Balance - amount < -CreditLimit)
            throw new AccountException("You cannot exceed the credit limit");
        Balance -= amount;
    }

    public void ChangeCreditLimit(decimal value)
    {
        if (value < MinCreditLimit)
            throw new AccountException("Credit limit must be non-negative number");
        CreditLimit = value;
    }

    public void ChangeAnnualInterestRate(decimal value)
    {
        if (value < MinAnnualInterestRate)
            throw new AccountException("Annual Interest Rate must be non-negative number");
        AnnualInterestRate = value;
    }

    public void Recalculate()
    {
        if (Balance >= 0)
            return;
        Balance += -Balance * AnnualInterestRate / NumberOfPercentagesInOneWhole / DaysInYear;
    }

    public string Info()
    {
        return $"Owner                : {Owner.FirstName} {Owner.SecondName}\n" +
               $"Type                 : {Type}\n" +
               $"Balance              : {Balance}\n" +
               $"Credit Limit         : {CreditLimit}\n" +
               $"Annual interest rate : {AnnualInterestRate}\n" +
               $"Open date            : {OpenDate}\n" +
               $"Id                   : {Id}";
    }
}