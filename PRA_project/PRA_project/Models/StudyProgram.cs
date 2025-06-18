using System;
using System.Collections.Generic;

namespace PRA_project.Models;

public partial class StudyProgram
{
    public int IdStudyProgram { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
