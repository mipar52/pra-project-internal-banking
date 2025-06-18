using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class RequestTransfer
{
    public int IdRequestTransfer { get; set; }

    public int UserRecieverId { get; set; }

    public int UserSenderId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public virtual User UserReciever { get; set; } = null!;

    public virtual User UserSender { get; set; } = null!;
}
