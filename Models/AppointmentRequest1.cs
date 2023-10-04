using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class AppointmentRequest1
{
    public int AppointmentRequestId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? RequestDate { get; set; }

    public int? DeptId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Department? Dept { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
