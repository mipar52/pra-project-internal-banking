using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class ScholarshipPayment
{
    public int IdScholarshipPayment { get; set; }

    public int UserId { get; set; }

    public int StudyProgramId { get; set; }

    public string? PaymentPlan { get; set; }

    public int? InstallmentNumber { get; set; }

    public int? TotalInstallments { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual StudyProgram StudyProgram { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
