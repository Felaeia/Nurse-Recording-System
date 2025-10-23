namespace NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm
{
    public interface IViewUserForm
    {
        Task<ViewUserFormResponseDTO> GetUserFormAsync(int formId);
    }
}
