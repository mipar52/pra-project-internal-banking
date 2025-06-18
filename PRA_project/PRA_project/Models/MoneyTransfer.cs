using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class MoneyTransfer
{
    public int IdMoneyTransfer { get; set; }

    public int? TransactionId { get; set; }

    public int? UserRecieverId { get; set; }

    public virtual Transaction? Transaction { get; set; }

    public virtual User? UserReciever { get; set; }
}
