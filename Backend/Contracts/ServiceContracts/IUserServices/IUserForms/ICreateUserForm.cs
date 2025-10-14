using NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms
{
    public interface ICreateUserForm
    {
        Task<UserFormResponseDTO> CreateUserFormAsync(UserFormRequestDTO userForm, string userId, string creator);
    }
}
