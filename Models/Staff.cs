using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int? RoleId { get; set; }

    public string? StaffName { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public int? DeptId { get; set; }

    public string? Position { get; set; }

    public DateTime? JoiningDate { get; set; }

    public string? Password { get; set; }

    public string? StaffImg { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual Role? Role { get; set; }
}
