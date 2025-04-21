using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class UserCreditCard
{
    public int IduserCreditCard { get; set; }

    public int UserId { get; set; }

    public int CreditCardId { get; set; }

    public virtual CreditCard CreditCard { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
