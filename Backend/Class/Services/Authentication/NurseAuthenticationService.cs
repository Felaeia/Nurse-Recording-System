using NurseRecordingSystem.Model.DTO.HelperDTOs;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class NurseAuthenticationService
    {
        private readonly string? _connectionString;

        public NurseAuthenticationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        //Nurse Method: Login
        public async Task<LoginResponseDTO> NurseLogin()
        {
            // Implementation for Nurse Login
            throw new NotImplementedException();
        }

        //Nurse Method: Forgot password

        //Nurse Method: Reset password

        //Nurse Method: Change password

        //Nurse Method: Send code to verification email

        //Nurse Method: Vereify code from email

        //Nurse Method: 

    }
}
