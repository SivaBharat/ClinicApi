using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public int? RoleId { get; set; }

    public string? PatientName { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? ContactNumber { get; set; }

    public string? Adress { get; set; }

    public string? Email { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string? GuardianName { get; set; }

    public string? GuardianContactNumber { get; set; }

    public string? Password { get; set; }

    public string? PatientImg { get; set; }

    public virtual ICollection<AppointmentRequest1> AppointmentRequest1s { get; set; } = new List<AppointmentRequest1>();

    public virtual ICollection<AppointmentRequest> AppointmentRequests { get; set; } = new List<AppointmentRequest>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Role? Role { get; set; }
}
