namespace Banks.Interfaces;

public interface ITransaction
{
     int Id { get; }
     string Type { get; }
     IAccount Account { get; }
     DateTime DateTime { get; }
     decimal Amount { get; }
     void Cancel();
}