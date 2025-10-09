using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users
{
    public interface ICreateUsers
    {
        Task<int> CreateUserAuthenticateAsync(CreateAuthenticationRequestDTO authRequest, CreateUserRequestDTO user);
        Task CreateUserAsync(CreateUserRequestDTO user);
    }
}
