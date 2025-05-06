using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class SendRequestMoney : Transaction
{
    public int UserRecieverId { get; set; } // UserSender je onaj User u Transaction klasi
    public virtual User UserReciever { get; set; } = null!;
}
