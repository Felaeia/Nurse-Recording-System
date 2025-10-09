using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms
{
    public interface ICreateUserForm
    {
        Task<UserFormResponseDTO> CreateUserForm(UserFormRequestDTO userForm, string userId, string creator);
    }
}
