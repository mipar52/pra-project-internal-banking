using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class TransactionType
{
    public int IdTransactionType { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SendRequestMoney> SendRequestMoneys { get; set; } = new List<SendRequestMoney>();
}
