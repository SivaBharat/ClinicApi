using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class DoctorsController : ControllerBase
    {
        private readonly ClinicContext _context;
        public DoctorsController(ClinicContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            return await _context.Doctors.ToListAsync();
        }
       
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }
      
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {            
            doctor.RoleId = 2;
            doctor.JoiningDate = DateTime.Now;
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDoctor", new { id = doctor.DoctorId }, doctor);
        }

        [HttpPost("SendMail")]
        public ActionResult SendMail([FromBody] EmailResponse emailresponse)
        {
            if (ModelState.IsValid)
            {
                if (emailresponse == null)
                {
                    return BadRequest("Invalid request");
                }
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Siva","20bsca150vigneshr@skacas.ac.in"));
                    message.To.Add(MailboxAddress.Parse(emailresponse.Email));
                    message.Subject = "ACCOUNT CREATED";
                    var text = new TextPart("plain")
                    {
                        Text = $@"Your Account has been created the Health clinic
                 Details:
                 Your Password:{emailresponse.Password}
                 We expect a great determination and hard work towards the Clinic , All the Best...",
                    };
                    var multipart = new Multipart("mixed")
             {
                 text
             };
                    message.Body = multipart;
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("20bsca150vigneshr@skacas.ac.in", "welcome123");
                        client.Send(message);
                        client.Disconnect(true);
                    }
                    return new JsonResult(new { message = "Email sent Successfully" });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Content("Send Email method executed");
        }
       
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return (_context.Doctors?.Any(e => e.DoctorId == id)).GetValueOrDefault();
        }
    }
}
