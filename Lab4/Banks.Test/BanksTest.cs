using Banks.Entities;
using Banks.Entities.Accounts;
using Banks.Models;
using Banks.Models.ClientBuilder;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    private ClientDirector _director = new ();
    [Fact]
    public void MoneyAccumulationTest()
    {
        Client client = _director.BuildClient_OnlyName(1, "Me", "Me");
        Bank bank = CentralBank.GetInstance().CreateBank("tinkoff", 5000, new decimal(3.7), 3, 5);
        bank.AddClient(client);
        CustomDateTime.SetNow(DateTime.Parse("02/12/2024"));
        DepositAccount account = bank.CreateDepositAccount(client, DateTime.Parse("02/08/2025"));
        bank.Replenish(account, 1000);
        var dateTime = DateTime.Parse("03/08/2025");
        TimeSpan difference = dateTime - CustomDateTime.Now;
        for (int days = difference.Days; days > 0; days--)
        {
            CustomDateTime.SetNow(CustomDateTime.Now.Add(TimeSpan.FromDays(1)));
            bank.Update();
        }

        Assert.InRange(account.Balance, 1046.8M, 1046.81M);

        dateTime = DateTime.Parse("04/12/2025");
        difference = dateTime - CustomDateTime.Now;
        for (int days = difference.Days; days > 0; days--)
        {
            CustomDateTime.SetNow(CustomDateTime.Now.Add(TimeSpan.FromDays(1)));
            bank.Update();
        }

        Assert.InRange(account.Balance, 1046.8M, 1046.81M); // As you see money doesn't accumulate after end date
    }

    [Fact]
    public void MoneyTransfer_TransactionCancel()
    {
        Client firstClient = _director.BuildClient_OnlyName(2, "Me", "Me");
        Client secondClient = _director.BuildClient_OnlyName(3, "NotMe", "NotMe");
        Bank tinkoff = CentralBank.GetInstance().CreateBank("Tinkoff", 5000, new decimal(3.7), 3, 5);
        DebitAccount firstAcc = tinkoff.CreateDebitAccount(firstClient);
        Bank sber = CentralBank.GetInstance().CreateBank("Sber", 5000, new decimal(3.7), 3, 5);
        DebitAccount secondAcc = sber.CreateDebitAccount(secondClient);

        tinkoff.Replenish(firstAcc, 10000);
        int id = tinkoff.Transfer(firstAcc, secondAcc, 5000).Id;

        Assert.Equal(5000, firstAcc.Balance);
        Assert.Equal(5000, secondAcc.Balance);

        tinkoff.CancelTransaction(id);

        Assert.Equal(10000, firstAcc.Balance);
        Assert.Equal(0, secondAcc.Balance);
    }
}