using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.IUserForms
{
    public interface IUpdateUserForm
    {
        /// <summary>
        /// Updates the details of an existing patient form.
        /// </summary>
        /// <param name="userFormRequest">The DTO containing the updated form data.</param>
        /// <returns>A Task that represents the asynchronous operation, returning the UserFormResponseDTO.</returns>
        Task<UserFormResponseDTO> UpdateUserFormAsync(UserFormRequestDTO userFormRequest);
    }
}
