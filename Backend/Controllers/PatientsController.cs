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
        public async Task<IActionResult> PutPatient(int id, [FromBody]string patientRecord)
        {
            // ID exists
            if (!_context.Patients.Any(p => p.Id == id))
            {
                return NotFound();
            }

            try
            {
                Patient patient = TransformPatient(patientRecord);
                patient.Id = id;
                _context.Entry(patient).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                    // Transform and add to database
                    _context.Patients.Add(TransformPatient(rec));
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

        private Patient TransformPatient(string patientRecord)
        {
            // Retrieve values from record
            string[] record = patientRecord.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim()).ToArray();
            if (record.Length < 4)
            {
                // Incomplete record
                throw new ArgumentException("Incomplete record", patientRecord);
            }

            // Transform record

            // Birthday
            if (!DateTime.TryParse(record[2], out DateTime birthday))
                throw new ArgumentException("Invalid birthday", patientRecord);

            // Gender
            Gender gender;
            if (record[3].Trim() == "M")
                gender = Gender.Male;
            else if (record[3].Trim() == "F")
                gender = Gender.Female;
            else
                throw new ArgumentException("Invalid gender", patientRecord);

            return new Patient
            {
                FirstName = record[0],
                LastName = record[1],
                Birthday = birthday,
                Gender = gender
            };

            // ADD: include patient record info in thrown exception
        }
    }
}
