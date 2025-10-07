using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using NurseRecordingSystem.Contracts.ServiceContracts.User;
using NurseRecordingSystem.Model.DTO.UserDTOs;

namespace NurseRecordingSystem.Class.Services.UserServices
{
    public class UserFormService : IUserFormService
    {
        private readonly string? _connectionString;

        public UserFormService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        public async Task<UserFormResponseDTO> CreateUserForm(UserFormRequestDTO userFormRequest, string userId, string creator)
        {
            if (userFormRequest == null)
            {
                throw new ArgumentNullException(nameof(userFormRequest), "UserFormRequest Cannot be Null");
            }

            await using (var connecttion =  new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand("INSERT INTO [PatientForms](issueType, issueDescryption, status, userId, patientName, createdBy, updatedBy, deletedBy, isActive) " +
                "VALUES (@IssueType, @IssueDescryption, @Status, @UserId, @PatientName, @CreatedBy, @UpdatedBy, @DeletedBy, @IsActive) ", connecttion))
            {
                
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
