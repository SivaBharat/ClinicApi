namespace Clinic.Models;

public partial class AppointmentRequest
{
    public int AppointmentRequestId { get; set; }
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public DateTime? RequestDate { get; set; }
    public virtual Doctor? Doctor { get; set; }
    public virtual Patient? Patient { get; set; }
}
