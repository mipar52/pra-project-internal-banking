using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class ScholarshipPayment : Transaction
{
    public int StudyProgramId { get; set; }
    public string? PaymentPlan { get; set; }
    public int? InstallmentNumber { get; set; }
    public int? TotalInstallments { get; set; }
    public virtual StudyProgram StudyProgram { get; set; } = null!;
}
