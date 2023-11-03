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
    [TypeFilter(typeof(CustomExceptionFilter))]
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
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == request.Username && a.Password == request.Password);

            if (admin != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = admin.RoleId, userId = admin.AdminId });
            }            
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Email == request.Username && d.Password == request.Password);

            if (doctor != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = doctor.RoleId, userId = doctor.DoctorId });
            }            
            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.Email == request.Username && s.Password == request.Password);

            if (staff != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = staff.RoleId, userId = staff.StaffId, departmentId = staff.DeptId });
            }            
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == request.Username && p.Password == request.Password);

            if (patient != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                return Ok(new { success = true, roleId = patient.RoleId, userId = patient.PatientId });
            }           
            return Ok(new { success = false });
        }
    }
}
