using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class CreditCard
{
    public int IdCreditCard { get; set; }

    public string FirstName { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string CreditCardNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public string Cvvhash { get; set; } = null!;

    public string Cvvsalt { get; set; } = null!;

    public virtual ICollection<UserCreditCard> UserCreditCards { get; set; } = new List<UserCreditCard>();
}
