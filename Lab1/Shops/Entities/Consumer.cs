namespace Shops.Entities;

public class Consumer
{
    public const int MinAmountOfMoney = 0;
    public Consumer(string name, int amountOfMoney = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Consumer's name cannot be empty");
        if (amountOfMoney < MinAmountOfMoney)
            throw new ArgumentException("Amount of money must be non-negative");

        Name = name;
        AmountOfMoney = amountOfMoney;
    }

    public string Name { get; }
    public int AmountOfMoney { get; private set; }

    public void TakeMoney(int value)
    {
        if (value < MinAmountOfMoney)
            throw new ArgumentException("Amount of money must be non-negative");
        if (AmountOfMoney - value < MinAmountOfMoney)
            throw new ArgumentException("Cannot take money");
        AmountOfMoney -= value;
    }

    public void GiveMoney(int value)
    {
        if (value < MinAmountOfMoney)
            throw new ArgumentException("Amount of money must be non-negative");
        AmountOfMoney += value;
    }
}