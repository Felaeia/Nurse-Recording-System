// File: NurseRecordingSystem.Contracts.ServiceContracts.User/IUpdateUserService.cs

using System.Threading.Tasks;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.User
{
    public interface IUpdateUser
    {
        /// <summary>
        /// Updates the user's profile information based on the provided request data.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile is being updated.</param>
        /// <param name="userRequest">The DTO containing the new profile data.</param>
        /// <param name="updatedBy">The identifier of the user performing the update (for auditing).</param>
        /// <returns>A Task that represents the asynchronous operation. The task result is true if the update was successful (SP returned 0); otherwise, false.</returns>
        Task<bool> UpdateUserProfileAsync(int userId, UpdateUserRequestDTO userRequest, string updatedBy);
    }
}