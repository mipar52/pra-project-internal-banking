using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class ParkingPayment
{
    public int IdParkingPayment { get; set; }

    public int? TransactionId { get; set; }

    public string RegistrationNumber { get; set; } = null!;

    public string RegistrationCountryCode { get; set; } = null!;

    public int DurationHours { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
