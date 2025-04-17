using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class CanteenPayment
{
    public int IdCanteenPayment { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual User User { get; set; } = null!;
}
