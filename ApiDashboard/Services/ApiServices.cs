using ApiDemoShared.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace ApiDashboard.Services
{
    public interface IApiServices
    {
        Task<List<PatientDto>> GetPatientList();
        Task<bool> UpdatePatient(PatientDto model);
        Task<bool> DeletePatient(PatientDto model);
        Task<PatientDto> AddPatient(PatientDto model);
    }
    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;
        public ApiServices(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<List<PatientDto>> GetPatientList()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Patient/GetPatients");

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<List<PatientDto>>(results);
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> UpdatePatient(PatientDto model)
        {
            try
            {
                var response = await _httpClient.PutAsync("api/Patient/GetPatients", new StringContent(JsonConvert.SerializeObject(model)));

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<PatientDto> AddPatient(PatientDto model)
        {
            try
            {
                var response = await _httpClient.PostAsync("api/Patient/CreatePatient", new StringContent(JsonConvert.SerializeObject(model)));

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<PatientDto>(results);
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> DeletePatient(PatientDto model)
        {
            try
            {
                var response = await _httpClient.PostAsync("api/Patient/CreatePatient", new StringContent(JsonConvert.SerializeObject(model)));

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<PatientDto>(results);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
