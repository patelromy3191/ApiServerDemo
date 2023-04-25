using ApiDemoShared.DbModels;
using ApiDemoShared.DTOs;
using ApiServerDemo.Models;
using Dapper;
using System.Data;

namespace ApiServerDemo.Repository
{
    public interface IPatientRepository
    {
        Task<IEnumerable<PatientDto>> GetPatients();
        Task<Patient> GetPatientById(string id);
        Task<Patient> CreatePatient(PatientDto patient);
        Task UpdatePatient(PatientDto patient);
        Task DeletePatient(string id);
    }
    public class PatientRepository : IPatientRepository
    {
        private readonly ValleyHospitalContext context;

        public PatientRepository(ValleyHospitalContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<PatientDto>> GetPatients()
        {
            var query = "SELECT * FROM Patient";

            using (var connection = context.CreateConnection())
            {
                var companies = await connection.QueryAsync<PatientDto>(query);
                return companies.ToList();
            }
        }
        public async Task<Patient> GetPatientById(string id)
        {
            var patientId = Guid.Parse(id);
            var query = "SELECT * FROM Patient WHERE Id = @Id";

            using (var connection = context.CreateConnection())
            {
                var patient = await connection.QuerySingleOrDefaultAsync<Patient>(query, new { id });

                return patient;
            }
        }
        public async Task<Patient> CreatePatient(PatientDto patient)
        {
            patient.Id = Guid.NewGuid();
            patient.CreatedOn = DateTime.UtcNow;
            var query = "INSERT INTO Patient (Id, Name, Address, Email, CreatedOn) VALUES (@Id,@Name, @Address, @Email,@CreatedOn)";

            var parameters = new DynamicParameters();
            parameters.Add("Id", patient.Id, DbType.Guid);
            parameters.Add("Name", patient.Name, DbType.String);
            parameters.Add("Address", patient.Address, DbType.String);
            parameters.Add("Email", patient.Email, DbType.String);
            parameters.Add("CreatedOn", patient.CreatedOn, DbType.String);

            using (var connection = context.CreateConnection())
            {
                var id = await connection.QueryAsync(query, parameters);
                
                var newPatient = new Patient
                {
                    Id = Guid.NewGuid(),
                    Name = patient.Name,
                    Address = patient.Address,
                    Email = patient.Email,
                    CreatedOn = DateTime.UtcNow,
                };

                return newPatient;
            }
        }
        public async Task UpdatePatient(PatientDto patient)
        {
            var query = "UPDATE Patient SET Name = @Name, Address = @Address, Email = @Email, UpdatedOn = @UpdatedOn WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", patient.Id, DbType.Guid);
            parameters.Add("Name", patient.Name, DbType.String);
            parameters.Add("Address", patient.Address, DbType.String);
            parameters.Add("Email", patient.Email, DbType.String);
            parameters.Add("UpdatedOn", DateTime.UtcNow, DbType.String);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task DeletePatient(string id)
        {
            var query = "DELETE FROM Patient WHERE Id = @Id";

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

    }
}
