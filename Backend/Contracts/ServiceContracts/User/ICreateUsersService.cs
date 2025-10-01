using NurseRecordingSystem.Model.DTO.AuthDTOs;
using NurseRecordingSystem.Model.DTO.UserDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.User
{
    public interface ICreateUsersService
    {
        Task<int> CreateUserAuthenticateAsync(CreateAuthenticationRequestDTO authRequest, CreateUserRequestDTO user);
        Task CreateUser(CreateUserRequestDTO user);
    }
}
