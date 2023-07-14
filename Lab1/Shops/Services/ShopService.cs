using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

namespace Shops.Services;

public class ShopService : IShopService
{
    private const int MinPrice = 0;
    private const int MinAmount = 0;

    private List<Shop> _shops;
    private List<Consumer> _consumers;

    public ShopService()
    {
        _shops = new List<Shop>();
        _consumers = new List<Consumer>();
    }

    public Shop AddShop(string name, string address)
    {
        if (name is null)
            throw new ArgumentException("Name cannot be null");
        if (address is null)
            throw new ArgumentException("Address cannot be null");
        if (_shops.FindIndex(x => x.Name.Equals(name) && x.Address.Equals(address)) != -1)
            throw new ArgumentException("This shop already exists");

        var shop = new Shop(name, address);
        _shops.Add(shop);
        return _shops[^1];
    }

    public Shop AddShop(Shop shop)
    {
        if (shop is null)
            throw new ArgumentException("Shop cannot be null");
        if (_shops.FindIndex(x => x.Name.Equals(shop.Name) && x.Address.Equals(shop.Address)) != -1)
            throw new ExistenceException("This shop already exists");

        _shops.Add(shop);
        return _shops[^1];
    }

    public int GetShopId(Shop shop)
    {
        if (shop is null)
            throw new ArgumentException("Shop cannot be null");

        int index = _shops.FindIndex(x => x.Equals(shop));
        if (index == -1)
            throw new ExistenceException("This shop does not exists");
        return index - 1;
    }

    public int GetShopId(string name, string address)
    {
        if (name is null)
            throw new ArgumentException("Name cannot be null");
        if (address is null)
            throw new ArgumentException("Address cannot be null");

        int index = _shops.FindIndex(x => x.Name.Equals(name) && x.Address.Equals(address));
        if (index == -1)
            throw new ExistenceException("This shop does not exists");
        return index + 1;
    }

    public Shop GetShop(int id)
    {
        if (_shops.Count < id || id <= 0)
            throw new IdException("Invalid id");
        return _shops[id - 1];
    }

    public Consumer AddConsumer(string name, int amountOfMoney)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Consumer's name cannot be empty");
        if (amountOfMoney < Consumer.MinAmountOfMoney)
            throw new ArgumentException("Amount of money must be non-negative");

        var consumer = new Consumer(name, amountOfMoney);
        _consumers.Add(consumer);
        return _consumers[^1];
    }

    public Consumer AddConsumer(Consumer consumer)
    {
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");

        _consumers.Add(consumer);
        return _consumers[^1];
    }

    public Shop? FindBestShop(string name, int amount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        var product = new Product(name);
        List<Shop> shops = _shops.FindAll(x => x.Products.ContainsKey(product));
        shops.Sort((x, y) => x.Products[product].Price.CompareTo(y.Products[product].Price));

        return shops.FirstOrDefault(t => t.AmountOfProduct(product) >= amount);
    }

    public Shop? FindBestShop(Product product, int amount)
    {
        if (product is null)
            throw new ArgumentException("Product cannot be null");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        List<Shop> shops = _shops.FindAll(x => x.Products.ContainsKey(product));
        shops.Sort((x, y) => x.Products[product].Price.CompareTo(y.Products[product].Price));

        return shops.FirstOrDefault(t => t.AmountOfProduct(product) >= amount);
    }

    public void Buy(Shop shop, Consumer consumer, Product product, int amount)
    {
        if (shop is null)
            throw new ArgumentException("Shop cannot be null");
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");
        if (product is null)
            throw new ArgumentException("Product cannot be null");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        if (shop.TryBuy(consumer, product, amount))
            return;
        throw new AvailabilityException("Cannot buy");
    }

    public void Buy(Shop shop, Consumer consumer, string name, int amount)
    {
        if (shop is null)
            throw new ArgumentException("Shop cannot be null");
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        if (shop.TryBuy(consumer, name, amount))
            return;
        throw new AvailabilityException("Cannot buy");
    }

    public void BuyForTheBestPrice(Consumer consumer, Product product, int amount)
    {
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");
        if (product is null)
            throw new ArgumentException("Product cannot be null");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        Shop? shop = FindBestShop(product, amount);
        if (shop is null)
            throw new AvailabilityException("There is not enough product in any of the store");
        if (shop.TryBuy(consumer, product, amount))
            return;
        throw new AvailabilityException("Cannot buy");
    }

    public void BuyForTheBestPrice(Consumer consumer, string name, int amount)
    {
        if (consumer is null)
            throw new ArgumentException("Consumer cannot be null");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of product cannot be empty");
        if (amount < MinAmount)
            throw new AmountException("Amount of product must be non-negative");

        Shop? shop = FindBestShop(name, amount);
        if (shop is null)
            throw new AvailabilityException("There is not enough product in any of the store");
        if (shop.TryBuy(consumer, name, amount))
            return;
        throw new AvailabilityException("Cannot buy");
    }

    public Shop GetShopWithListForTheBestPrice(List<Product> products)
    {
        int? minSum = null;
        Shop? resultShop = null;
        foreach (Shop shop in _shops)
        {
            int sum = 0;
            foreach (Product product in products)
            {
                if (shop.AmountOfProduct(product) == 0)
                    break;
                sum += shop.PriceOfProduct(product);
            }

            if (minSum is null && sum != 0)
            {
                minSum = sum;
                resultShop = shop;
            }

            if (sum < minSum)
            {
                minSum = sum;
                resultShop = shop;
            }
        }

        if (resultShop is null)
            throw new AvailabilityException("There is no shops with this products");
        return resultShop;
    }
}
