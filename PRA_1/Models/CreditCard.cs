using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class CreditCard
{
    public int IdcreditCard { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public string CvvHash { get; set; } = null!;

    public string CvvSalt { get; set; } = null!;

    public virtual ICollection<UserCreditCard> UserCreditCards { get; set; } = new List<UserCreditCard>();
}
