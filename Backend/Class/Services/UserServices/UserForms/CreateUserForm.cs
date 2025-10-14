using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
using NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Class.Services.UserServices.UserForms
{
    public class CreateUserForm : ICreateUserForm
    {
        private readonly string? _connectionString;

        public CreateUserForm(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        public async Task<UserFormResponseDTO> CreateUserFormAsync(UserFormRequestDTO userFormRequest, string userId, string creator)
        {
            if (userFormRequest == null)
            {
                throw new ArgumentNullException(nameof(userFormRequest), "UserFormRequest Cannot be Null");
            }


            await using (var connecttion =  new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("dbo.usp_CreateUserForm ", connecttion))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@issueType", userFormRequest.issueType);
                cmd.Parameters.AddWithValue("@issueDescryption", userFormRequest.issueDescryption);
                cmd.Parameters.AddWithValue("@Status", userFormRequest.status);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@PatientName", userFormRequest.patientName);
                cmd.Parameters.AddWithValue("@CreatorName", creator);
                cmd.Parameters.AddWithValue("@UpdaterName", userFormRequest.updatedBy);
                cmd.Parameters.AddWithValue("@DeletedBy", userFormRequest.DeletedBy);
                cmd.Parameters.AddWithValue("@IsActive", 1);

                try
                {
                    await connecttion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                } catch (SqlException ex)
                {
                    throw new Exception("Error in Create User Form", ex);
                }

                return new UserFormResponseDTO
                {

                };
            }
        }
    }
}
