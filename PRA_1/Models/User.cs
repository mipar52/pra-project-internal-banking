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

    public virtual ICollection<FoodPayment> FoodPayments { get; set; } = new List<FoodPayment>();

    public virtual ICollection<ParkingPayment> ParkingPayments { get; set; } = new List<ParkingPayment>();

    public virtual ICollection<ScholarshipPayment> ScholarshipPayments { get; set; } = new List<ScholarshipPayment>();

    public virtual ICollection<SendRequestMoney> SendRequestMoneyUserRecievers { get; set; } = new List<SendRequestMoney>();

    public virtual ICollection<SendRequestMoney> SendRequestMoneyUserSenders { get; set; } = new List<SendRequestMoney>();

    public virtual ICollection<UserAllTransaction> UserAllTransactions { get; set; } = new List<UserAllTransaction>();

    public virtual ICollection<UserCarRegistration> UserCarRegistrations { get; set; } = new List<UserCarRegistration>();

    public virtual ICollection<UserCreditCard> UserCreditCards { get; set; } = new List<UserCreditCard>();

    public virtual ICollection<UserFriend> UserFriendFriends { get; set; } = new List<UserFriend>();

    public virtual ICollection<UserFriend> UserFriendUsers { get; set; } = new List<UserFriend>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
