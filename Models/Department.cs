﻿using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Department
{
    public int DeptId { get; set; }

    public string? DeptName { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}