using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class UserCarRegistration
{
    public int IduserCarRegistration { get; set; }

    public int UserId { get; set; }

    public int CarRegistrationId { get; set; }

    public virtual CarRegistration CarRegistration { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
