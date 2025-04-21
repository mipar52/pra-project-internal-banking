using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class AllTransaction
{
    public int IdAllTransactions { get; set; }

    public decimal TransactionPayment { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual ICollection<UserAllTransaction> UserAllTransactions { get; set; } = new List<UserAllTransaction>();
}
