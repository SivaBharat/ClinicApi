using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public int? RoleId { get; set; }

    public string? DoctorName { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public int? DeptId { get; set; }

    public string? Qualification { get; set; }

    public string? VisitingDays { get; set; }

    public DateTime? JoiningDate { get; set; }

    public byte[]? DoctorImg { get; set; }

    public virtual ICollection<AppointmentRequest> AppointmentRequests { get; set; } = new List<AppointmentRequest>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Department? Dept { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Role? Role { get; set; }
}
