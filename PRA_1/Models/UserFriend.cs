using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class UserFriend
{
    public int IdUserFriends { get; set; }

    public int UserId { get; set; }

    public int FriendId { get; set; }

    public virtual User Friend { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
