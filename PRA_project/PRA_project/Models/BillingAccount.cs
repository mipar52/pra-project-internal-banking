using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class BillingAccount
{
    public int IdBillingAccount { get; set; }

    public int? UserId { get; set; }

    public decimal Balance { get; set; }

    public virtual User? User { get; set; }
}
