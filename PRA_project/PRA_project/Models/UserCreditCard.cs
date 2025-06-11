using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class UserCreditCard
{
    public int IdUserCreditCard { get; set; }

    public int? UserId { get; set; }

    public int? CreditCardId { get; set; }

    public virtual CreditCard? CreditCard { get; set; }

    public virtual User? User { get; set; }
}
