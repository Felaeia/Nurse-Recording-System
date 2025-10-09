using NurseRecordingSystem.Model.DTO.HelperDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.Auth
{
    public interface IUserAuthenticationService
    {
        Task<LoginResponseDTO?> AuthenticateAsync(LoginRequestDTO request);
        Task<int> DetermineRoleAync(LoginResponseDTO response);
        Task LogoutAsync();
    }
}
