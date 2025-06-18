using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class TransactionType
{
    public int IdTransactionType { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
