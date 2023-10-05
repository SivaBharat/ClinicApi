using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Models;
using Microsoft.Data.SqlClient;

namespace Clinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginRequestController : ControllerBase
    {
        private readonly ClinicContext _context;

        public LoginRequestController(ClinicContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data");
            }

            // Check if the credentials are in the Admin table
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == request.Username && a.Password == request.Password);

            if (admin != null)
            {
                // Credentials are valid
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = admin.RoleId, userId = admin.AdminId });
            }

            // Check if the credentials are in the Doctors table
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Email == request.Username && d.Password == request.Password);

            if (doctor != null)
            {
                // Credentials are valid
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = doctor.RoleId, userId = doctor.DoctorId });
            }

            // Check if the credentials are in the Staff table
            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.Email == request.Username && s.Password == request.Password);

            if (staff != null)
            {
                // Credentials are valid
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = staff.RoleId, userId = staff.StaffId, departmentId = staff.DeptId });
            }

            // Check if the credentials are in the Patients table
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == request.Username && p.Password == request.Password);

            if (patient != null)
            {
                // Credentials are valid
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = patient.RoleId, userId = patient.PatientId });
            }

            // Invalid credentials
            return Ok(new { success = false });
        }
    }
}
