using Shops.Entities;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class ShopsTests
{
    private ShopService _shops = new ShopService();
    [Fact]
    public void PutProductsInTheShop_ProductsInTheSystemAndProductsCanBeBought()
    {
        Shop shop = _shops.AddShop("711", "Lenina st., 5");
        var product = new Product("milk");

        shop.Supply(product, 70, 100);
        shop.Supply(product, 70, 100);

        Assert.Equal(200, shop.AmountOfProduct(product));

        var consumer = new Consumer("me", 140);
        shop.TryBuy(consumer, product, 2);

        Assert.Equal(0, consumer.AmountOfMoney);
        Assert.Equal(198, shop.AmountOfProduct(product));
    }

    [Fact]
    public void SettingAndChangingPrice()
    {
        Shop shop = _shops.AddShop("711", "Lenina st., 5");

        var product = new Product("milk");
        shop.Supply(product, 70, 100);
        Assert.Equal(70, shop.PriceOfProduct(product));

        shop.SetPrice(product, 80);
        Assert.Equal(80, shop.PriceOfProduct(product));
    }

    [Fact]
    public void FindShopWithTheBestPriceAndEnoughProduct()
    {
        Shop firstShop = _shops.AddShop("711", "Lenina st., 5");
        Shop secondShop = _shops.AddShop("Lenta", "Random st., 7");
        Shop thirdShop = _shops.AddShop("Magnit", "Hz st., 9");

        var product = new Product("milk");
        firstShop.Supply(product, 100, 100);
        secondShop.Supply(product, 10, 10);
        thirdShop.Supply(product, 50, 50);

        Assert.Equal(thirdShop, _shops.FindBestShop(product, 40));
    }

    [Fact]
    public void BuyProductLot()
    {
        Shop shop = _shops.AddShop("711", "Lenina st., 5");
        var order = new List<(Product product, int amount)>();

        var milk = new Product("milk");
        var meat = new Product("meat");
        var cheese = new Product("cheese");
        var chocolate = new Product("chocolate");
        var juice = new Product("juice");

        shop.Supply(milk, 70, 100);
        shop.Supply(meat, 700, 100);
        shop.Supply(cheese, 300, 100);
        shop.Supply(chocolate, 80, 1);
        shop.Supply(juice, 100, 100);

        order.Add((milk, 100));
        order.Add((meat, 100));
        order.Add((cheese, 100));
        order.Add((chocolate, 100));
        order.Add((juice, 100));

        var consumer = new Consumer("Me", 200000);

        foreach ((Product product, int amount) product in order)
            shop.TryBuy(consumer, product.product, product.amount);

        Assert.Equal(0, shop.AmountOfProduct(milk));
        Assert.Equal(0, shop.AmountOfProduct(meat));
        Assert.Equal(0, shop.AmountOfProduct(cheese));
        Assert.Equal(1, shop.AmountOfProduct(chocolate));
        Assert.Equal(0, shop.AmountOfProduct(juice));
        Assert.Equal(83000, consumer.AmountOfMoney);
    }
}