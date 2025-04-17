using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class User
{
    public int Iduser { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? TestPassword { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Temp2Facode { get; set; }

    public DateTime? Temp2FacodeExpires { get; set; }

    public virtual ICollection<BillingAccount> BillingAccounts { get; set; } = new List<BillingAccount>();

    public virtual ICollection<CanteenPayment> CanteenPayments { get; set; } = new List<CanteenPayment>();

    public virtual ICollection<ParkingPayment> ParkingPayments { get; set; } = new List<ParkingPayment>();

    public virtual ICollection<ScholarshipPayment> ScholarshipPayments { get; set; } = new List<ScholarshipPayment>();

    public virtual ICollection<UserCarRegistration> UserCarRegistrations { get; set; } = new List<UserCarRegistration>();

    public virtual ICollection<UserCreditCard> UserCreditCards { get; set; } = new List<UserCreditCard>();
}
