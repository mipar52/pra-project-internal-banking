using System;
using System.Collections.Generic;

namespace PRA_1.Models;

public partial class StudyProgram
{
    public int IdStudyProgram { get; set; }

    public string Name { get; set; } = null!;

    public string? DegreeLevel { get; set; }

    public string? Institution { get; set; }

    public decimal FullScholarshipPrice { get; set; }

    public virtual ICollection<ScholarshipPayment> ScholarshipPayments { get; set; } = new List<ScholarshipPayment>();
}
