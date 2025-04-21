using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class CarRegistration
{
    public int IdcarRegistration { get; set; }

    public string? CarBrand { get; set; }

    public string? CarModel { get; set; }

    public string RegistrationCountry { get; set; } = null!;

    public string RegistrationNumber { get; set; } = null!;

    public virtual ICollection<UserCarRegistration> UserCarRegistrations { get; set; } = new List<UserCarRegistration>();
}
