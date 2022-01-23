using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientRecordsContext _context;

        public PatientsController(PatientRecordsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all patient records
        /// </summary>
        /// <returns>List of patients</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPatients()
        {
            return await _context.Patients
                .Select(p => new
                {
                    id = p.Id,
                    firstName = p.FirstName,
                    lastName = p.LastName,
                    birthday = p.Birthday.ToString("yyyy-MM-dd"),
                    gender = p.Gender.ToString()
                }).ToListAsync();
        }

        /// <summary>
        /// Update Patient by ID
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="patient">Patient record</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        public async Task<ActionResult> PostPatients(List<Patient> patients)
        {
            try
            {
                _context.Patients.AddRange(patients);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return Ok();
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadPatientsFile([FromBody]string file)
        {
            string[] records = file.Split(new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            try
            {
                // Iterate through records
                foreach (string rec in records)
                {
                    // Retrieve values from record
                    string[] record = rec.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(r => r.Trim()).ToArray();
                    if (record.Length < 4)
                    {
                        // Incomplete record
                        throw new ArgumentException("Incomplete record");
                    }

                    // Transform record

                    // Birthday
                    if (!DateTime.TryParse(record[2], out DateTime birthday))
                        throw new ArgumentException("Invalid birthday");

                    // Gender
                    Gender gender;
                    if (record[3].Trim() == "M")
                        gender = Gender.Male;
                    else if (record[3].Trim() == "F")
                        gender = Gender.Female;
                    else
                        throw new ArgumentException("Invalid gender");
                    
                    // Add to database
                    _context.Patients.Add(new Patient
                    {
                        FirstName = record[0],
                        LastName = record[1],
                        Birthday = birthday,
                        Gender = gender
                    });
                }

                // Save context
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex);
            }

            return Ok();
        }

        /// <summary>
        /// Delete patient by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// TEST METHOD - REMOVE AFTER
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAllPatients()
        {
            _context.Patients.RemoveRange(_context.Patients);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
