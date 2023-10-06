using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public DoctorsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            return await _context.Doctors.ToListAsync();
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
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

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        //// POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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
        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
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
