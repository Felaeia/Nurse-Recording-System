using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class CreatePatientRecord : ICreatePatientRecord
    {
        private readonly string? _connectionString;

        public CreatePatientRecord(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreatePatientRecordAsync(CreatePatientRecordRequestDTO request, string createdBy)
        {
            const string storedProc = "dbo.nsp_CreatePatientRecord";
            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nursingDiagnosis", request.NursingDiagnosis);
                cmd.Parameters.AddWithValue("@nursingIntervention", request.NursingIntervention);
                cmd.Parameters.AddWithValue("@nurseId", request.NurseId);
                cmd.Parameters.AddWithValue("@createdBy", createdBy);

                try
                {
                    await connection.OpenAsync();
                    // ExecuteScalar is efficient for returning the identity value (PatientRecordId)
                    var result = await cmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
                    {
                        throw new InvalidOperationException("Stored procedure did not return the new PatientRecordId.");
                    }

                    return Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating patient record.", ex);
                }
            }
        }
    }
}