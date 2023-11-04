using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    [Authorize]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly ClinicContext _context;
        public MedicalRecordsController(ClinicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecords()
        {
            if (_context.MedicalRecords == null)
            {
                return NotFound();
            }
            return await _context.MedicalRecords.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int id)
        {
            if (_context.MedicalRecords == null)
            {
                return NotFound();
            }
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return medicalRecord;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalRecord(int id, MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.RecordId)
            {
                return BadRequest();
            }
            _context.Entry(medicalRecord).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalRecordExists(id))
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
        public async Task<ActionResult<MedicalRecord>> PostMedicalRecord(MedicalRecord medicalRecord)
        {
            if (_context.MedicalRecords == null)
            {
                return Problem("Entity set 'ClinicContext.MedicalRecords'  is null.");
            }
            medicalRecord.VisitDate=DateTime.Now;
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetMedicalRecord", new { id = medicalRecord.RecordId }, medicalRecord);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            if (_context.MedicalRecords == null)
            {
                return NotFound();
            }
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }
            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MedicalRecordExists(int id)
        {
            return (_context.MedicalRecords?.Any(e => e.RecordId == id)).GetValueOrDefault();
        }
    }
}
