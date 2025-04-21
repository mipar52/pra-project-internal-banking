using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class UserRole
{
    public int IduserRole { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
