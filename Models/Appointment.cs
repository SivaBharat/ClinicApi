using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? AppointmentRequestId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? AppointmentProvidedDate { get; set; }

    public int? TokenNumber { get; set; }

    public TimeSpan? AppointmentTime { get; set; }

    public virtual AppointmentRequest? AppointmentRequest { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
