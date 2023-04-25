using ApiDemoShared.DTOs;
using ApiServerDemo.Repository;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("GetPatients")]
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
            if (patient == null)
                return StatusCode(452, "No Parameter Found");
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
        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(PatientDto patientDto)
        {
            if (patientDto == null)
                return StatusCode(452, "No Parameter Found");
            try
            {
                await patientRepo.UpdatePatient(patientDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("DeletePatient")]
        public async Task<IActionResult> DeletePatient(PatientDto patientDt)
        {
            try
            {
                if (patientDt == null)
                    return StatusCode(452, "No Parameter Found");
                var dbCompany = await patientRepo.GetPatientById(patientDt.Id.ToString());
                if (dbCompany == null)
                    return NotFound();

                await patientRepo.DeletePatient(patientDt.Id.ToString());
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
