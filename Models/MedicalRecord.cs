using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? VisitDate { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? Prescription { get; set; }

    public string? Remark { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
