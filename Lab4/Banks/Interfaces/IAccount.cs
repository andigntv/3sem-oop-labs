using Banks.Entities;
using Banks.Models;

namespace Banks.Interfaces;

 public interface IAccount
{
     int Id { get; }
     string Type { get; }
     DateTime OpenDate { get; }
     decimal Balance { get; }
     Client Owner { get; }
     decimal WithdrawingLimit { get; }
     void Replenishment(decimal amount);
     void Transfer(decimal amount, IAccount anotherAccount);
     void Withdrawing(decimal amount);
     void Recalculate();
     string Info();
}