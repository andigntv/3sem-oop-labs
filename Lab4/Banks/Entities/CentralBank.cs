using Banks.Exceptions;

namespace Banks.Entities;

public class CentralBank
{
    private static CentralBank _centralBank = new ();
    private List<Bank> _banks = new ();
    private CentralBank() { }
    public IReadOnlyList<Bank> Banks => _banks;
    public static CentralBank GetInstance()
    {
        return _centralBank;
    }

    public Bank CreateBank(string name, decimal creditLimit, decimal creditRate, decimal interestOnBalance, decimal cumulativeInterest)
    {
        if (FindBank(name) is not null)
            throw new BankException("Bank with this name already exists");

        var bank = new Bank(name, creditLimit, creditRate, interestOnBalance, cumulativeInterest);
        _banks.Add(bank);
        return bank;
    }

    public Bank? FindBank(string? name)
    {
        return _banks.Find(x => x.Name.Equals(name));
    }

    public void Update()
    {
        foreach (Bank bank in _banks)
            bank.Update();
    }
}