using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Clinic.CustomAuthorize;

namespace Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class AppointmentRequest1Controller : ControllerBase
    {
        private readonly ClinicContext _context;

        public AppointmentRequest1Controller(ClinicContext context)
        {
            _context = context;
        }

        [Block]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentRequest1>>> GetAppointmentRequests1()
        {
          if (_context.AppointmentRequests1 == null)
          {
              return NotFound();
          }
            return await _context.AppointmentRequests1.ToListAsync();
        }

        [Block]
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

        [Block]
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

        [Block]
        [HttpPost]
        public async Task<ActionResult<AppointmentRequest1>> PostAppointmentRequest1(AppointmentRequest1 appointmentRequest1)
        {
            appointmentRequest1.Status = 0;
            _context.AppointmentRequests1.Add(appointmentRequest1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentRequest1", new { id = appointmentRequest1.AppointmentRequestId }, appointmentRequest1);
        }

        [Block]
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
