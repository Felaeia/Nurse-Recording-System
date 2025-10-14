using NurseRecordingSystem.DTO.AuthServiceDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.Auth
{
    public interface IUserAuthenticationService
    {
        Task<LoginResponseDTO?> AuthenticateAsync(LoginRequestDTO request);
        Task<int> DetermineRoleAync(LoginResponseDTO response);
        Task LogoutAsync();
    }
}
