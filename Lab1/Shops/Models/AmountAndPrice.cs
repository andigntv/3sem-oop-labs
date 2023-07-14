namespace Shops.Models;

public class AmountAndPrice
{
    private const int MinPrice = 0;
    private const int MinAmount = 0;

    public AmountAndPrice(int price, int amount = 0)
    {
        if (price < MinPrice)
            throw new ArgumentException("Price of product must be non-negative");
        if (amount < MinAmount)
            throw new ArgumentException("Amount of products must be non-negative");

        Price = price;
        Amount = amount;
    }

    public int Price { get; private set; }
    public int Amount { get; private set; }

    public AmountAndPrice Clone()
    {
        return new AmountAndPrice(Price, Amount);
    }

    public void SetPrice(int newPrice)
    {
        if (newPrice >= MinPrice)
            Price = newPrice;
    }

    public void SetAmount(int newAmount)
    {
        if (newAmount >= MinAmount)
            Amount = newAmount;
    }
}