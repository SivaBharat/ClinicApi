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
using Clinic.CustomAuthorize;

namespace Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class StaffsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public StaffsController(ClinicContext context)
        {
            _context = context;
        }

        [Block]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
        {
            if (_context.Staff == null)
            {
                return NotFound();
            }
            return await _context.Staff.ToListAsync();
        }

        [Block]
        [HttpGet("{id}")]
        public async Task<ActionResult<Staff>> GetStaff(int id)
        {
            if (_context.Staff == null)
            {
                return NotFound();
            }
            var staff = await _context.Staff.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        [Block]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, Staff staff)
        {
            if (id != staff.StaffId)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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

        [Block]
        [HttpPost]
        public async Task<ActionResult<Staff>> PostStaff(Staff staff)
        {            
            staff.RoleId = 3;
            staff.JoiningDate = DateTime.Now;
            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaff", new { id = staff.StaffId }, staff);
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
                    message.From.Add(new MailboxAddress("Siva", "20bsca150vigneshr@skacas.ac.in"));
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
        [Block]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            if (_context.Staff == null)
            {
                return NotFound();
            }
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StaffExists(int id)
        {
            return (_context.Staff?.Any(e => e.StaffId == id)).GetValueOrDefault();
        }
    }
}
