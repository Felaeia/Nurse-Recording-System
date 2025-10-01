using NurseRecordingSystem.Model.DTO.UserDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.User
{
    public interface IUserFormService
    {
        Task<UserFormResponseDTO> CreateUserForm(UserFormRequestDTO userForm, string userId, string creator);
    }
}
