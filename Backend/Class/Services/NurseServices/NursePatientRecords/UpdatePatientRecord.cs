using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class UpdatePatientRecord : IUpdatePatientRecord
    {
        private readonly string? _connectionString;

        public UpdatePatientRecord(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<UpdatePatientRecordResponseDTO> UpdatePatientRecordAsync(UpdatePatientRecordRequestDTO request, string updatedBy)
        {
            const string storedProc = "dbo.nsp_UpdatePatientRecord";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@patientRecordId", request.PatientRecordId);
                cmd.Parameters.AddWithValue("@nursingDiagnosis", request.NursingDiagnosis);
                cmd.Parameters.AddWithValue("@nursingIntervention", request.NursingIntervention);
                cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException($"Patient record with ID {request.PatientRecordId} was not found or could not be updated.");
                    }

                    return new UpdatePatientRecordResponseDTO
                    {
                        Success = true,
                        UpdatedRecordId = request.PatientRecordId
                    };
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 50002) // Custom SP error number
                {
                    throw new KeyNotFoundException(sqlEx.Message, sqlEx);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating patient record {request.PatientRecordId}.", ex);
                }
            }
        }
    }
}