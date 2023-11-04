﻿namespace Clinic.Models;

public partial class Admin
{
    public int AdminId { get; set; }
    public int? RoleId { get; set; }
    public string? Password { get; set; }
    public string? Username { get; set; }
    public virtual Role? Role { get; set; }
}
