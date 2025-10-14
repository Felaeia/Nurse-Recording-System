using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers
{
    public interface IViewAllUsers
    {
        // Assuming a DTO for a list of users
        Task<List<ViewUserDTO>> GetAllUsersAsync(bool? isActive);
    }
}
