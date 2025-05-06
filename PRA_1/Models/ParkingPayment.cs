using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class ParkingPayment : Transaction
{
    public string RegistrationCountryCode { get; set; } = null!;
    public string RegistrationNumber { get; set; } = null!;
    public int DurationHours { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
