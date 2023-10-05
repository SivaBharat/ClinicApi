using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;

namespace Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentRequest1Controller : ControllerBase
    {
        private readonly ClinicContext _context;

        public AppointmentRequest1Controller(ClinicContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentRequest1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentRequest1>>> GetAppointmentRequests1()
        {
          if (_context.AppointmentRequests1 == null)
          {
              return NotFound();
          }
            return await _context.AppointmentRequests1.ToListAsync();
        }

        // GET: api/AppointmentRequest1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentRequest1>> GetAppointmentRequest1(int id)
        {
          if (_context.AppointmentRequests1 == null)
          {
              return NotFound();
          }
            var appointmentRequest1 = await _context.AppointmentRequests1.FindAsync(id);

            if (appointmentRequest1 == null)
            {
                return NotFound();
            }

            return appointmentRequest1;
        }

        // PUT: api/AppointmentRequest1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentRequest1(int id, AppointmentRequest1 appointmentRequest1)
        {
            if (id != appointmentRequest1.AppointmentRequestId)
            {
                return BadRequest();
            }

            _context.Entry(appointmentRequest1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentRequest1Exists(id))
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

        // POST: api/AppointmentRequest1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppointmentRequest1>> PostAppointmentRequest1(AppointmentRequest1 appointmentRequest1)
        {
            appointmentRequest1.Status = 0;
            _context.AppointmentRequests1.Add(appointmentRequest1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentRequest1", new { id = appointmentRequest1.AppointmentRequestId }, appointmentRequest1);
        }

        // DELETE: api/AppointmentRequest1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentRequest1(int id)
        {
            if (_context.AppointmentRequests1 == null)
            {
                return NotFound();
            }
            var appointmentRequest1 = await _context.AppointmentRequests1.FindAsync(id);
            if (appointmentRequest1 == null)
            {
                return NotFound();
            }

            _context.AppointmentRequests1.Remove(appointmentRequest1);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentRequest1Exists(int id)
        {
            return (_context.AppointmentRequests1?.Any(e => e.AppointmentRequestId == id)).GetValueOrDefault();
        }
    }
}
