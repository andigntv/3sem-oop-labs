using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop
{
    private const int MinPrice = 0;
    private const int MinAmount = 0;
    private const int MinAmountOfMoney = 0;
    private const int DefaultMoneyAmount = 1000000;

    private Dictionary<Product, AmountAndPrice> _products;

    public Shop(string name, string address, int amountOfMoney = DefaultMoneyAmount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of shop cannot be empty");
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty");

        Name = name;
        Address = address;
        AmountOfMoney = amountOfMoney;
        _products = new Dictionary<Product, AmountAndPrice>();
    }

    public string Name { get; }
    public string Address { get; }
    public int AmountOfMoney { get; private set; }
    public IReadOnlyDictionary<Product, AmountAndPrice> Products => _products;

    public void GiveMoney(int amountOfMoney)
    {
        if (amountOfMoney < MinAmountOfMoney)
            throw new AmountException("Amount of products must be non-negative");
        AmountOfMoney += amountOfMoney;
    }

    public void Supply(string name, int price, int amount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");
        if (price < MinPrice)
            throw new PriceException("Price of product must be non-negative");
        if (amount < MinAmount)
            throw new AmountException("Amount of products must be non-negative");
        if (amount * price > AmountOfMoney)
            throw new AvailabilityException("Shop cannot pay supply");

        var product = new Product(name);
        if (_products.ContainsKey(product))
        {
            AmountAndPrice amountAndPrice = _products[product];
            amountAndPrice.SetAmount(amountAndPrice.Amount + amount);
            if (price > amountAndPrice.Price)
                amountAndPrice.SetPrice(price);
            return;
        }

        _products.Add(product, new AmountAndPrice(price, amount));
    }

    public void Supply(Product product, int price, int amount)
    {
        if (product is null)
            throw new ArgumentException("Product cannot be null");
        if (price < MinPrice)
            throw new PriceException("Price of product must be non-negative");
        if (amount < MinAmount)
            throw new AmountException("Amount of products must be non-negative");
        if (amount * price > AmountOfMoney)
            throw new AvailabilityException("Shop cannot pay supply");

        AmountOfMoney -= amount * price;
        if (_products.ContainsKey(product))
        {
            AmountAndPrice amountAndPrice = _products[product];
            amountAndPrice.SetAmount(amountAndPrice.Amount + amount);
            if (price > amountAndPrice.Price)
                amountAndPrice.SetPrice(price);
            return;
        }

        _products.Add(product, new AmountAndPrice(price, amount));
    }

    public void SetPrice(string name, int newPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");
        if (newPrice < MinPrice)
            throw new PriceException("Price of product must be non-negative");

        var product = new Product(name);
        if (_products.ContainsKey(product) is false)
            throw new AvailabilityException("Shop does not have this product");
        _products[product].SetPrice(newPrice);
    }

    public void SetPrice(Product product, int newPrice)
    {
        if (product is null)
            throw new ArgumentException("Product cannot be null");
        if (newPrice < MinPrice)
            throw new PriceException("Price of product must be non-negative");

        if (_products.ContainsKey(product) is false)
            throw new AvailabilityException("Shop does not have this product");
        _products[product].SetPrice(newPrice);
    }

    public AmountAndPrice InformationAboutProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");

        var product = new Product(name);
        return _products[product].Clone();
    }

    public AmountAndPrice InformationAboutProduct(Product product)
    {
        if (product is null)
            throw new ArgumentException("Product cannot be null");

        return _products[product].Clone();
    }

    public int AmountOfProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");

        var product = new Product(name);
        return _products.ContainsKey(product) ? _products[product].Amount : 0;
    }

    public int AmountOfProduct(Product product)
    {
        if (product is null)
            throw new ArgumentException("Product cannot be null");

        return _products.ContainsKey(product) ? _products[product].Amount : 0;
    }

    public int PriceOfProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");

        var product = new Product(name);
        if (_products.ContainsKey(product))
            return _products[product].Price;
        throw new AvailabilityException("Shop does not have this product");
    }

    public int PriceOfProduct(Product product)
    {
        if (product is null)
            throw new ArgumentException("Product cannot be null");

        if (_products.ContainsKey(product))
            return _products[product].Price;
        throw new ArgumentException("Invalid products name");
    }

    public bool TryBuy(Consumer consumer, string name, int amount)
    {
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        var product = new Product(name);
        int sum = _products[product].Price * amount;
        if ((consumer.AmountOfMoney < sum) || (_products[product].Amount < amount)) return false;

        consumer.TakeMoney(sum);
        GiveMoney(sum);
        _products[product].SetAmount(_products[product].Amount - amount);
        return true;
    }

    public bool TryBuy(Consumer consumer, Product product, int amount)
    {
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");
        if (product is null)
            throw new ArgumentException("Product cannot be null");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        int sum = _products[product].Price * amount;
        if ((consumer.AmountOfMoney < sum) || (_products[product].Amount < amount)) return false;

        consumer.TakeMoney(sum);
        GiveMoney(sum);
        _products[product].SetAmount(_products[product].Amount - amount);
        return true;
    }
}
