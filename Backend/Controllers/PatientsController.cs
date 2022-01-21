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
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        /// <summary>
        /// Get Patient by ID
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns>Patient record</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
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

        /// <summary>
        /// Create new patient
        /// </summary>
        /// <param name="patient">Patient details in request body</param>
        /// <returns>ID of created patient</returns>
        //[HttpPost]
        //public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        //{
        //    _context.Patients.Add(patient);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        //}

        [HttpPost]
        public async Task<ActionResult> PostPatients(List<Patient> patients)
        {
            // iterate through all patients
            // check if id already exists
            try
            {
                // Auto increment id 
                patients.ForEach(p => p.Id = 0);
                _context.Patients.AddRange(patients);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

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

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
