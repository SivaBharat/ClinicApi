using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Clinic.Controllers
{
    #nullable disable
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
                var token = GenerateJwtToken((int)admin.RoleId, admin.AdminId, null);
                return Ok(new { success = true, token });
            }            
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Email == request.Username && d.Password == request.Password);

            if (doctor != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                var token = GenerateJwtToken((int)doctor.RoleId, doctor.DoctorId, doctor.DeptId);
                return Ok(new { success = true, token });
            }            
            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.Email == request.Username && s.Password == request.Password);

            if (staff != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                var token = GenerateJwtToken((int)staff.RoleId, staff.StaffId, staff.DeptId);
                return Ok(new { success = true, token });
            }            
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == request.Username && p.Password == request.Password);

            if (patient != null)
            {                
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteExpiredAppointments");
                var token = GenerateJwtToken((int)patient.RoleId, patient.PatientId, null);
                return Ok(new { success = true, token });
            }           
            return Ok(new { success = false });
        }
        private string GenerateJwtToken(int roleId, int userId, int? departmentId)
        {
            var userrole = "no role";
            if(roleId == 1)
            {
                userrole = "Admin";
            }
            else if(roleId == 2)
            {
                userrole = "Doctor";
            }
            else if (roleId == 3)
            {
                userrole = "Staff";
            }
            else if (roleId == 4)
            {
                userrole = "Patient";
            }
            else
            {
                userrole = "User has no role";
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("roleId", roleId.ToString()),
                new Claim(ClaimTypes.Role, userrole)
            };
            if (departmentId.HasValue)
            {
                claims = claims.Append(new Claim("departmentId", departmentId.ToString())).ToArray();
            }
            var token = new JwtSecurityToken(                
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
