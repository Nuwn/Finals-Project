using System;
using System.Collections.Generic;

namespace Finals_API.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Salt { get; set; }

    public bool Activated { get; set; }

    public bool Locked { get; set; }

    public string? EmailNormalized { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual Score? Score { get; set; }
}
