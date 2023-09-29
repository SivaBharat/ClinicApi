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
    public class AppointmentRequestsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public AppointmentRequestsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentRequest>>> GetAppointmentRequests()
        {
            if (_context.AppointmentRequests == null)
            {
                return NotFound();
            }
            return await _context.AppointmentRequests.ToListAsync();
        }

        // GET: api/AppointmentRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentRequest>> GetAppointmentRequest(int id)
        {
            if (_context.AppointmentRequests == null)
            {
                return NotFound();
            }
            var appointmentRequest = await _context.AppointmentRequests.FindAsync(id);

            if (appointmentRequest == null)
            {
                return NotFound();
            }

            return appointmentRequest;
        }

        // PUT: api/AppointmentRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentRequest(int id, AppointmentRequest appointmentRequest)
        {
            if (id != appointmentRequest.AppointmentRequestId)
            {
                return BadRequest();
            }

            _context.Entry(appointmentRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentRequestExists(id))
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

        // POST: api/AppointmentRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppointmentRequest>> PostAppointmentRequest(AppointmentRequest appointmentRequest)
        {
            if (_context.AppointmentRequests == null)
            {
                return Problem("Entity set 'ClinicContext.AppointmentRequests'  is null.");
            }
            _context.AppointmentRequests.Add(appointmentRequest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AppointmentRequestExists(appointmentRequest.AppointmentRequestId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAppointmentRequest", new { id = appointmentRequest.AppointmentRequestId }, appointmentRequest);
        }

        // DELETE: api/AppointmentRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentRequest(int id)
        {
            if (_context.AppointmentRequests == null)
            {
                return NotFound();
            }
            var appointmentRequest = await _context.AppointmentRequests.FindAsync(id);
            if (appointmentRequest == null)
            {
                return NotFound();
            }

            _context.AppointmentRequests.Remove(appointmentRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentRequestExists(int id)
        {
            return (_context.AppointmentRequests?.Any(e => e.AppointmentRequestId == id)).GetValueOrDefault();
        }
    }
}
