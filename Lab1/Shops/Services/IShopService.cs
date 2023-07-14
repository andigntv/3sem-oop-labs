using Shops.Entities;

namespace Shops.Services;

public interface IShopService
{
    public Shop AddShop(string name, string address);
    public Shop AddShop(Shop shop);
    public int GetShopId(Shop shop);
    public int GetShopId(string name, string address);
    public Shop GetShop(int id);
    public Consumer AddConsumer(string name, int amountOfMoney);
    public Consumer AddConsumer(Consumer consumer);
    public Shop? FindBestShop(string name, int amount);
    public Shop? FindBestShop(Product product, int amount);
    public void Buy(Shop shop, Consumer consumer, Product product, int amount);
    public void Buy(Shop shop, Consumer consumer, string name, int amount);
    public void BuyForTheBestPrice(Consumer consumer, Product product, int amount);
    public void BuyForTheBestPrice(Consumer consumer, string name, int amount);
    public Shop GetShopWithListForTheBestPrice(List<Product> products);
}