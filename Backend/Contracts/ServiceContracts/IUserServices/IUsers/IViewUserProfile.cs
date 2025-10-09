using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.Users
{
    public interface IViewUserProfile
    {
        /// <summary>
        /// Retrieves the profile details for a specific user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the UserProfileDTO.</returns>
        Task<ViewUserProfileDTO> GetUserProfileAsync(int userId);
    }
}
