﻿using System.ComponentModel.DataAnnotations;

namespace Finals_API.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Account
{
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string? Username { get; set; }

}

