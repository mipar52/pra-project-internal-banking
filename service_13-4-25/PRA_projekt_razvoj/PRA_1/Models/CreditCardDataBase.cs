using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class CreditCardDataBase
{
    public int IdcreditCardDataBase { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public string CvvHash { get; set; } = null!;

    public string CvvSalt { get; set; } = null!;
}
