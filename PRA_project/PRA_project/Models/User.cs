using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class User
{
    public int IdUser { get; set; }

    public int? RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int? StudyProgramId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public DateTime? Temp2FacodeExpires { get; set; }

    public string? Temp2Facode { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string LastName { get; set; } = null!;

    public virtual ICollection<BillingAccount> BillingAccounts { get; set; } = new List<BillingAccount>();

    public virtual ICollection<Friend> FriendFriendNavigations { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    public virtual ICollection<MoneyTransfer> MoneyTransfers { get; set; } = new List<MoneyTransfer>();

    public virtual ICollection<RequestTransfer> RequestTransferUserRecievers { get; set; } = new List<RequestTransfer>();

    public virtual ICollection<RequestTransfer> RequestTransferUserSenders { get; set; } = new List<RequestTransfer>();

    public virtual Role? Role { get; set; }

    public virtual StudyProgram? StudyProgram { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserCreditCard> UserCreditCards { get; set; } = new List<UserCreditCard>();
}
