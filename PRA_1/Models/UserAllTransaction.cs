using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class UserAllTransaction
{
    public int IduserAllTransactions { get; set; }

    public int UserId { get; set; }

    public int TransactionTypeId { get; set; }

    public int TransactionId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public virtual User User { get; set; } = null!;
}
