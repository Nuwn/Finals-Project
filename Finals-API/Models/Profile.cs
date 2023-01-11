using System;
using System.Collections.Generic;

namespace Finals_API.Models;

public partial class Profile
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
