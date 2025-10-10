namespace NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms
{
    public interface IDeleteUserForm
    {
        /// <summary>
        /// Performs a soft-delete on a PatientForm record.
        /// </summary>
        /// <param name="formId">The ID of the form to delete.</param>
        /// <param name="deletedBy">The identifier of the user performing the deletion (for auditing).</param>
        /// <returns>A Task representing the asynchronous operation. The result is true if the deletion was successful (SP returned 0), false otherwise.</returns>
        Task<bool> DeleteUserFormAsync(int formId, string deletedBy);
    }
}