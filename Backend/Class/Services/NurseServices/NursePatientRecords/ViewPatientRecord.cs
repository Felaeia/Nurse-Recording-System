using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.PatientRecordsDTOs;


namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class ViewPatientRecord : IViewPatientRecord
    {
        private readonly string? _connectionString;

        public ViewPatientRecord(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ViewPatientRecordResponseDTO> GetPatientRecordAsync(int patientRecordId)
        {
            const string storedProc = "dbo.nsp_ViewPatientRecord";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@patientRecordId", patientRecordId);

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ViewPatientRecordResponseDTO
                            {
                                PatientRecordId = reader.GetInt32(reader.GetOrdinal("patientRecordId")),
                                NursingDiagnosis = reader.GetString(reader.GetOrdinal("nursingDiagnosis")),
                                NursingIntervention = reader.GetString(reader.GetOrdinal("nursingIntervention")),
                                NurseId = reader.GetInt32(reader.GetOrdinal("nurseId")),
                                CreatedOn = reader.GetDateTime(reader.GetOrdinal("createdOn")),
                                CreatedBy = reader.GetString(reader.GetOrdinal("createdBy")),
                                UpdatedOn = reader.IsDBNull(reader.GetOrdinal("updatedOn")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updatedOn")),
                                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updatedBy")) ? null : reader.GetString(reader.GetOrdinal("updatedBy")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("isActive"))
                            };
                        }
                        throw new KeyNotFoundException($"Patient record with ID {patientRecordId} not found or is archived.");
                    }
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 50001) // Custom SP error number
                {
                    throw new KeyNotFoundException(sqlEx.Message, sqlEx);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error retrieving patient record {patientRecordId}.", ex);
                }
            }
        }
    }
}