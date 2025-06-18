using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class Friend
{
    public int IdFriend { get; set; }

    public int? UserId { get; set; }

    public int? FriendId { get; set; }

    public virtual User? FriendNavigation { get; set; }

    public virtual User? User { get; set; }
}
