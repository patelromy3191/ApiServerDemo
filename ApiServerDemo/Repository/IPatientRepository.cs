using ApiServerDemo.Models;
using ApiServerDemo.Models.DbModels;
using ApiServerDemo.Models.DTOs;
using Dapper;
using System.Data;

namespace ApiServerDemo.Repository
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetPatients();
        Task<Patient> GetPatientById(string id);
        Task<Patient> CreatePatient(PatientDto patient);
        Task UpdatePatient(string id, PatientDto company);
        Task DeleteCompany(string id);
    }
    public class PatientRepository : IPatientRepository
    {
        private readonly ValleyHospitalContext context;

        public PatientRepository(ValleyHospitalContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            var query = "SELECT * FROM Patient";

            using (var connection = context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Patient>(query);
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
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Id", Guid.NewGuid(), DbType.String);
            parameters.Add("Name", patient.Name, DbType.String);
            parameters.Add("Address", patient.Address, DbType.String);
            parameters.Add("Email", patient.Email, DbType.String);

            using (var connection = context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                
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
        public async Task UpdatePatient(string id, PatientDto company)
        {
            var query = "UPDATE Patient SET Id = @Id, Name = @Name, Address = @Address, Email = @Email WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Email", company.Email, DbType.String);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task DeleteCompany(string id)
        {
            var query = "DELETE FROM Patient WHERE Id = @Id";

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

    }
}
