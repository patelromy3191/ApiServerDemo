using ApiServerDemo.Models.DbModels;
using ApiServerDemo.Models.DTOs;
using ApiServerDemo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ApiServerDemo.Controllers
{
    [Route("api/Patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepo;

        public PatientController(IPatientRepository patientRepo)
        {
            this.patientRepo = patientRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            try
            {
                var companies = await patientRepo.GetPatients();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}", Name = "GetPatientById")]
        public async Task<IActionResult> GetPatientById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();
            try
            {
                var patient = await patientRepo.GetPatientById(id);
                if (patient == null)
                    return NotFound();

                return Ok(patient);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreatePatient(PatientDto patient)
        {
            try
            {
                var createdCompany = await patientRepo.CreatePatient(patient);
                return CreatedAtRoute("PatientById", new { id = createdCompany.Id }, createdCompany);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(string id, PatientDto patientDto)
        {
            try
            {
                var patient = await patientRepo.GetPatientById(id);
                if (patient == null)
                    return NotFound();

                await patientRepo.UpdatePatient(id, patientDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            try
            {
                var dbCompany = await patientRepo.GetPatientById(id);
                if (dbCompany == null)
                    return NotFound();

                await patientRepo.DeleteCompany(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
