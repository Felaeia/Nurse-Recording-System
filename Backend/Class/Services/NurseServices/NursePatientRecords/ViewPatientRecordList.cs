using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class ViewPatientRecordList : IViewPatientRecordList
    {
        private readonly string? _connectionString;

        public ViewPatientRecordList(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<PatientRecordListItemDTO>> GetPatientRecordListAsync(int? nurseId = null)
        {
            var forms = new List<PatientRecordListItemDTO>();
            const string storedProc = "dbo.nsp_ViewPatientRecordList";

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nurseId", nurseId.HasValue ? (object)nurseId.Value : DBNull.Value);

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            forms.Add(new PatientRecordListItemDTO
                            {
                                PatientRecordId = reader.GetInt32(reader.GetOrdinal("patientRecordId")),
                                NursingDiagnosis = reader.GetString(reader.GetOrdinal("nursingDiagnosis")),
                                NursingIntervention = reader.GetString(reader.GetOrdinal("nursingIntervention")),
                                CreatedOn = reader.GetDateTime(reader.GetOrdinal("createdOn")),
                                CreatedBy = reader.GetString(reader.GetOrdinal("createdBy"))
                            });
                        }
                    }
                    return forms;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving the list of patient records.", ex);
                }
            }
        }
    }
}