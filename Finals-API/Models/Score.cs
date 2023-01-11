using System;
using System.Collections.Generic;

namespace Finals_API.Models;

public partial class Score
{
    public string UserId { get; set; } = null!;

    public int Solved { get; set; }

    public long Attempts { get; set; }

    public DateTime? LastScored { get; set; }

    public virtual User User { get; set; } = null!;
}
