﻿using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Department
{
    public int DeptId { get; set; }

    public string? DeptName { get; set; }

    public virtual ICollection<AppointmentRequest1> AppointmentRequest1s { get; set; } = new List<AppointmentRequest1>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
