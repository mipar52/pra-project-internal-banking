using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class SendRequestMoney
{
    public int IdSendRequestMoney { get; set; }

    public int UserSenderId { get; set; }

    public int UserRecieverId { get; set; }

    public int TransactionTypeId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual TransactionType TransactionType { get; set; } = null!;

    public virtual User UserReciever { get; set; } = null!;

    public virtual User UserSender { get; set; } = null!;
}
