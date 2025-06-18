using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class Transaction
{
    public int IdTransaction { get; set; }

    public int UserId { get; set; }

    public int TransactionTypeId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public virtual ICollection<MoneyTransfer> MoneyTransfers { get; set; } = new List<MoneyTransfer>();

    public virtual ICollection<ParkingPayment> ParkingPayments { get; set; } = new List<ParkingPayment>();

    public virtual TransactionType TransactionType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
