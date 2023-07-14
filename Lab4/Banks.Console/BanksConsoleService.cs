using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.ClientBuilder;

namespace Banks.Console;

public class BanksConsoleService
{
    private const decimal MinNonNegativeNumber = 0;
    private int _clientId = 1;

    public BanksConsoleService()
    {
    }

    public Bank? CurrentBank { get; private set; }
    public Client? CurrentClient { get; private set; }

    public Bank? FindBank(string? name)
    {
        return CentralBank.GetInstance().FindBank(name);
    }

    public void Main()
    {
        string? currentCommand = null;
        while (currentCommand is null || !currentCommand.Equals("--Quit"))
        {
            System.Console.WriteLine("Write one of commands below:");
            System.Console.WriteLine("--CreateBank");
            System.Console.WriteLine("--CreateClient");
            System.Console.WriteLine("--EnterAddress");
            System.Console.WriteLine("--EnterPassport");
            System.Console.WriteLine("--ChangeCurrentBank");
            System.Console.WriteLine("--ChangeCurrentClient");
            System.Console.WriteLine("--CreateCreditAccount");
            System.Console.WriteLine("--CreateDebitAccount");
            System.Console.WriteLine("--CreateDepositAccount");
            System.Console.WriteLine("--ListOfBanks");
            System.Console.WriteLine("--ListOfClients");
            System.Console.WriteLine("--ListOfAccounts");
            System.Console.WriteLine("--AccountInfo");
            System.Console.WriteLine("--BankInfo");
            System.Console.WriteLine("--ClientInfo");
            System.Console.WriteLine("--Withdraw money");
            System.Console.WriteLine("--Replenish account");
            System.Console.WriteLine("--Transfer money");
            System.Console.WriteLine("--Cancel transaction");
            System.Console.WriteLine("--Quit");
            currentCommand = System.Console.ReadLine();
            switch (currentCommand)
            {
                case "--CreateBank":
                    CreateBank();
                    break;
                case "--CreateClient":
                    CreateClient();
                    break;
                case "--EnterAddress":
                    EnterAddress();
                    break;
                case "--EnterPassport":
                    EnterPassport();
                    break;
                case "--ChangeCurrentBank":
                    ChangeBank();
                    break;
                case "--ChangeCurrentClient":
                    ChangeClient();
                    break;
                case "--CreateCreditAccount":
                    CreateCreditAccount();
                    break;
                case "--CreateDebitAccount":
                    CreateDebitAccount();
                    break;
                case "--CreateDepositAccount":
                    CreateDepositAccount();
                    break;
                case "--ListOfBanks":
                    ListOfBanks();
                    break;
                case "--ListOfClients":
                    ListOfClients();
                    break;
                case "--ListOfAccounts":
                    ListOfAccounts();
                    break;
                case "--AccountInfo":
                    AccountInfo();
                    break;
                case "--BankInfo":
                    BankInfo();
                    break;
                case "--ClientInfo":
                    ClientInfo();
                    break;
                case "--Withdraw money":
                    WithdrawMoney();
                    break;
                case "--Replenish account":
                    ReplenishAccount();
                    break;
                case "--Transfer money":
                    TransferMoney();
                    break;
                case "--Cancel transaction":
                    break;
            }
        }
    }

    public void CreateBank()
    {
        System.Console.Clear();
        string? name = null;
        while (name is null || !Validator.BankNameCheck(name))
        {
            System.Console.WriteLine(
                "Write name of the bank, this field cannot be empty and can't match the name of another bank");
            name = System.Console.ReadLine();
        }

        decimal creditRate = -1;
        while (creditRate <= MinNonNegativeNumber || decimal.TryParse(System.Console.ReadLine(), out creditRate))
            System.Console.WriteLine("Enter rate for credit accounts, it must be a non-negative number");

        decimal creditLimit = -1;
        while (creditLimit <= MinNonNegativeNumber || decimal.TryParse(System.Console.ReadLine(), out creditLimit))
            System.Console.WriteLine("Enter limit for credit accounts, it must be a non-negative number");

        decimal interestOnBalance = -1;
        while (interestOnBalance <= MinNonNegativeNumber || decimal.TryParse(System.Console.ReadLine(), out interestOnBalance))
            System.Console.WriteLine("Enter interest on balance for debit accounts, it must be a non-negative number");

        decimal cumulativeInterest = -1;
        while (cumulativeInterest <= MinNonNegativeNumber || decimal.TryParse(System.Console.ReadLine(), out cumulativeInterest))
            System.Console.WriteLine("Enter cumulative interest for deposit accounts, it must be a non-negative number");

        CurrentBank = CentralBank.GetInstance()
            .CreateBank(name, creditLimit, creditRate, interestOnBalance, cumulativeInterest);
    }

    public void CreateClient()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("Before creating a client, you need at least one bank");
            return;
        }

        string? firstName = null;
        while (firstName is null)
        {
            System.Console.WriteLine("Enter your first name");
            firstName = System.Console.ReadLine();
        }

        string? secondName = null;
        while (secondName is null)
        {
            System.Console.WriteLine("Enter your second name");
            secondName = System.Console.ReadLine();
        }

        string? passportAnswer = null;
        while (passportAnswer is null || !passportAnswer.Equals("y") || !passportAnswer.Equals("n"))
            System.Console.WriteLine("Would you like to enter your passport data? y/n");

        Passport? passport = null;
        if (passportAnswer.Equals("y"))
        {
            string? series = null;
            string? number = null;
            while (!Validator.PassportDataValidityCheck(series, number, CurrentBank)
                   || series is null
                   || number is null)
            {
                System.Console.WriteLine("Enter series: ");
                series = System.Console.ReadLine();
                System.Console.WriteLine("Enter number: ");
                number = System.Console.ReadLine();
            }

            passport = new Passport(series, number);
        }

        string? addressAnswer = null;
        while (addressAnswer is null || !addressAnswer.Equals("y") || !addressAnswer.Equals("n"))
            System.Console.WriteLine("Would you like to enter your address? y/n");

        Address? address = null;
        if (addressAnswer.Equals("y"))
        {
            string? city = null;
            string? street = null;
            string? building = null;
            while (!Validator.AddressValidityCheck(city, street, building)
                   || city is null
                   || street is null
                   || building is null)
            {
                System.Console.WriteLine("Enter city: ");
                city = System.Console.ReadLine();
                System.Console.WriteLine("Enter street: ");
                street = System.Console.ReadLine();
                System.Console.WriteLine("Enter building: ");
                building = System.Console.ReadLine();
            }

            address = new Address(city, street, building);
        }

        var director = new ClientDirector();
        if (passportAnswer.Equals("y") && addressAnswer.Equals("y") && passport is not null && address is not null)
            CurrentClient = director.BuildClient_Full(_clientId++, firstName, secondName, passport, address);

        if (passportAnswer.Equals("y") && addressAnswer.Equals("n") && passport is not null)
            CurrentClient = director.BuildClient_NameAndPassport(_clientId++, firstName, secondName, passport);

        if (passportAnswer.Equals("n") && addressAnswer.Equals("y") && address is not null)
            CurrentClient = director.BuildClient_NameAndAddress(_clientId++, firstName, secondName, address);

        if (passportAnswer.Equals("n") && addressAnswer.Equals("n"))
            CurrentClient = director.BuildClient_OnlyName(_clientId++, firstName, secondName);

        if (CurrentClient is null)
            return;
        CurrentBank.AddClient(CurrentClient);
    }

    public void ChangeBank()
    {
        System.Console.WriteLine("After changing bank, you will leave your account");
        System.Console.WriteLine("Chose one of the banks and enter name");
        ListOfBanks();

        Bank? bank = null;
        while (bank is null)
        {
            System.Console.WriteLine("Enter name of the bank you want to choose");
            bank = FindBank(System.Console.ReadLine());
        }

        CurrentClient = null;
        CurrentBank = bank;
    }

    public void ChangeClient()
    {
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentBank.Clients.Count == 0)
        {
            System.Console.WriteLine("You have to create at least one client");
            return;
        }

        ListOfClients();
        System.Console.WriteLine("Write id of client you want to choose");

        Client? client = null;
        while (!int.TryParse(System.Console.ReadLine(), out int clientId) || client is null)
        {
            System.Console.WriteLine("Enter id of client in current bank");
            client = CurrentBank.FindClient(clientId);
        }

        CurrentClient = client;
    }

    public void CreateCreditAccount()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        CurrentBank.CreateCreditAccount(CurrentClient);
    }

    public void CreateDebitAccount()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        CurrentBank.CreateDebitAccount(CurrentClient);
    }

    public void CreateDepositAccount()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        System.Console.WriteLine("Enter the account closing date(dd/mm/yyyy): ");
        DateTime endDateTime;
        while (!DateTime.TryParse(System.Console.ReadLine(), out endDateTime))
        {
            System.Console.WriteLine("Enter the account closing date(dd/mm/yyyy): ");
        }

        CurrentBank.CreateDepositAccount(CurrentClient, endDateTime);
    }

    public void WithdrawMoney()
    {
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        ListOfAccounts();
        System.Console.WriteLine("Select the account you want to withdraw money from, by entering the id");
        int id;
        while (!int.TryParse(System.Console.ReadLine(), out id)
               || id < MinNonNegativeNumber
               || CurrentBank.FindAccount(id) is null)
            System.Console.WriteLine("Select the account you want to withdraw money from, by entering the id");

        IAccount? account = CurrentBank.FindAccount(id);
        ArgumentNullException.ThrowIfNull(account);

        System.Console.Clear();
        System.Console.WriteLine($"Enter the amount you want to withdraw, max amount: {account.WithdrawingLimit}");
        decimal amount;
        while (!decimal.TryParse(System.Console.ReadLine(), out amount)
               || amount > account.WithdrawingLimit
               || amount < MinNonNegativeNumber)
            System.Console.WriteLine($"Enter the amount you want to withdraw, max amount: {account.WithdrawingLimit}");

        CurrentBank.Withdraw(account, amount);
    }

    public void ReplenishAccount()
    {
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        ListOfAccounts();
        System.Console.WriteLine("Select the account you want to replenish, by entering the id");
        int id;
        while (!int.TryParse(System.Console.ReadLine(), out id)
               || id < MinNonNegativeNumber
               || CurrentBank.FindAccount(id) is null)
            System.Console.WriteLine("Select the account you want to replenish, by entering the id");

        IAccount? account = CurrentBank.FindAccount(id);
        ArgumentNullException.ThrowIfNull(account);

        System.Console.Clear();
        System.Console.WriteLine("Enter the amount you want to replenish your account with");
        decimal amount;
        while (!decimal.TryParse(System.Console.ReadLine(), out amount)
               || amount < MinNonNegativeNumber)
            System.Console.WriteLine("Enter the amount you want to replenish your account with");

        CurrentBank.Replenish(account, amount);
    }

    public void TransferMoney()
    {
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        ListOfAccounts();
        System.Console.WriteLine("Select the account you want to transfer money from, by entering the id");
        int id;
        while (!int.TryParse(System.Console.ReadLine(), out id)
               || id < MinNonNegativeNumber
               || CurrentBank.FindAccount(id) is null)
            System.Console.WriteLine("Select the account you want to transfer money from, by entering the id");

        IAccount? senderAccount = CurrentBank.FindAccount(id);
        ArgumentNullException.ThrowIfNull(senderAccount);

        System.Console.Clear();
        System.Console.WriteLine($"You want transfer money to account in Bank {CurrentBank.Name}? y/n");
        string? answer = null;
        while (answer is null || !answer.Equals("y") || !answer.Equals("n"))
            System.Console.WriteLine($"You want transfer money to account in Bank {CurrentBank.Name}? y/n");

        System.Console.Clear();
        IAccount? recipientAccount;
        if (answer.Equals("y"))
        {
            foreach (IAccount account in CurrentBank.Accounts)
                System.Console.WriteLine($"{account.Type}\t\t{account.Id}\t\t{account.Balance}");
            System.Console.WriteLine("Choose one of accounts above, by entering id : ");

            while (!int.TryParse(System.Console.ReadLine(), out id)
                   || id < MinNonNegativeNumber
                   || CurrentBank.FindAccount(id) is null)
                System.Console.WriteLine("Choose one of accounts above, by entering the id");

            recipientAccount = CurrentBank.FindAccount(id);
        }
        else
        {
            foreach (Bank bank in CentralBank.GetInstance().Banks)
                System.Console.WriteLine(bank.Name);
            System.Console.WriteLine("Choose one of banks above, by entering its name : ");
            Bank? tempBank = null;
            while (tempBank is null)
                tempBank = FindBank(System.Console.ReadLine());

            foreach (IAccount account in tempBank.Accounts)
                System.Console.WriteLine($"{account.Type}\t\t{account.Id}\t\t{account.Balance}");
            System.Console.WriteLine("Choose one of accounts above, by entering id : ");

            while (!int.TryParse(System.Console.ReadLine(), out id)
                   || id < MinNonNegativeNumber
                   || tempBank.FindAccount(id) is null)
                System.Console.WriteLine("Select the account you want to transfer money from, by entering the id");

            recipientAccount = tempBank.FindAccount(id);
        }

        System.Console.Clear();
        System.Console.WriteLine($"Enter the amount you want to transfer, max amount {senderAccount.WithdrawingLimit}");
        decimal amount;
        while (!decimal.TryParse(System.Console.ReadLine(), out amount)
               || amount > senderAccount.WithdrawingLimit
               || amount < MinNonNegativeNumber)
            System.Console.WriteLine($"Enter the amount you want to transfer, max amount {senderAccount.WithdrawingLimit}");

        ArgumentNullException.ThrowIfNull(recipientAccount);
        CurrentBank.Transfer(senderAccount, recipientAccount, amount);
    }

    public void CancelTransaction()
    {
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        System.Console.WriteLine("Type\t\tDate\t\tAmount");
        foreach (ITransaction tempTransaction in CurrentBank.Transactions)
            System.Console.WriteLine($"{tempTransaction.Type}\t{tempTransaction.DateTime}\t{tempTransaction.Amount}");
        System.Console.WriteLine("Choose one of transactions above by entering it is id");
        int id;
        while (!int.TryParse(System.Console.ReadLine(), out id)
               || id <= MinNonNegativeNumber
               || CurrentBank.FindTransaction(id) is null)
            System.Console.WriteLine("Choose one of transactions above by entering it is id");

        CurrentBank.CancelTransaction(id);
    }

    public void SetDate()
    {
        System.Console.Clear();
        System.Console.WriteLine("Enter the date(dd/mm/yyyy): ");

        DateTime dateTime;
        while (!DateTime.TryParse(System.Console.ReadLine(), out dateTime))
        {
            System.Console.WriteLine("Enter the date(dd/mm/yyyy): ");
        }

        TimeSpan difference = dateTime - CustomDateTime.Now;
        for (int days = difference.Days; days > 0; days--)
        {
            CustomDateTime.SetNow(CustomDateTime.Now.Add(TimeSpan.FromDays(1)));
            foreach (Bank bank in CentralBank.GetInstance().Banks)
                bank.Update();
        }

        CustomDateTime.SetNow(dateTime);
    }

    public void ListOfBanks()
    {
        System.Console.Clear();
        if (CentralBank.GetInstance().Banks.Count == 0)
            return;
        System.Console.WriteLine("Banks: ");
        foreach (Bank bank in CentralBank.GetInstance().Banks)
            System.Console.WriteLine(bank.Name);
    }

    public void ListOfClients()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        System.Console.WriteLine("Clients in current bank: ");
        System.Console.WriteLine("First name\t\tSecond name\t\tid");
        foreach (Client client in CurrentBank.Clients)
            System.Console.WriteLine($"{client.FirstName}\t\t{client.SecondName}\t\t{client.Id}");
    }

    public void ListOfAccounts()
    {
        System.Console.Clear();
        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        System.Console.WriteLine("Accounts of current client: ");
        System.Console.WriteLine("Type\t\tid\t\tbalance\t\t");
        foreach (IAccount account in CurrentClient.Accounts)
            System.Console.WriteLine($"{account.GetType()}\t\t{account.Id}\t\t{account.Balance}");
    }

    public void BankInfo()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        System.Console.WriteLine($"Name                : {CurrentBank.Name}" +
                                 $"Credit limit        : {CurrentBank.CreditLimit}" +
                                 $"Credit rate         : {CurrentBank.CreditRate}" +
                                 $"Interest on balance : {CurrentBank.InterestOnBalance}" +
                                 $"Cumulative interest : {CurrentBank.CumulativeInterest}");
    }

    public void ClientInfo()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        string result = $"First name: {CurrentClient.FirstName}\n" +
                        $"Second name: {CurrentClient.SecondName}\n" +
                        $"Id: {CurrentClient.Id}";
        if (CurrentClient.Passport is not null)
        {
            result += $"\nPassport series: {CurrentClient.Passport.Series}\n" +
                      $"Passport number: {CurrentClient.Passport.Number}";
        }

        if (CurrentClient.Address is not null)
            result += $"\nAddress: {CurrentClient.Address.City}, {CurrentClient.Address.Street}, {CurrentClient.Address.Building}";
        System.Console.WriteLine(result);
    }

    public void AccountInfo()
    {
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        ListOfAccounts();
        System.Console.WriteLine("Select the account by entering the id");
        int id;
        while (!int.TryParse(System.Console.ReadLine(), out id)
               || id < MinNonNegativeNumber
               || CurrentBank.FindAccount(id) is null)
            System.Console.WriteLine("Select the account by entering the id");

        IAccount? account = CurrentBank.FindAccount(id);
        ArgumentNullException.ThrowIfNull(account);
        System.Console.WriteLine(account.Info());
    }

    public void EnterPassport()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        string? series = null;
        string? number = null;
        while (!Validator.PassportDataValidityCheck(series, number, CurrentBank)
               || series is null
               || number is null)
        {
            System.Console.WriteLine("Enter series: ");
            series = System.Console.ReadLine();
            System.Console.WriteLine("Enter number: ");
            number = System.Console.ReadLine();
        }

        CurrentClient.SetPassport(new Passport(series, number));
    }

    public void EnterAddress()
    {
        System.Console.Clear();
        if (CurrentBank is null)
        {
            System.Console.WriteLine("You have to create bank first");
            return;
        }

        if (CurrentClient is null)
        {
            System.Console.WriteLine("You have to create client first");
            return;
        }

        string? city = null;
        string? street = null;
        string? building = null;
        while (!Validator.AddressValidityCheck(city, street, building)
               || city is null
               || street is null
               || building is null)
        {
            System.Console.WriteLine("Enter city: ");
            city = System.Console.ReadLine();
            System.Console.WriteLine("Enter street: ");
            street = System.Console.ReadLine();
            System.Console.WriteLine("Enter building: ");
            building = System.Console.ReadLine();
        }

        CurrentClient.SetAddress(new Address(city, street, building));
    }
}