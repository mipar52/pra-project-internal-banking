using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class ParkingPayment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RegistrationCountryCode { get; set; } = null!;

    public string RegistrationNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public int DurationHours { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual User User { get; set; } = null!;
}
