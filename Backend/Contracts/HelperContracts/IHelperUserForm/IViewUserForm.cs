namespace NurseRecordingSystem.Contracts.HelperContracts.IHelperUserForm
{
    public interface IViewUserForm
    {
        Task<ViewUserFormResponseDTO> GetUserFormAsync(int formId);
    }
}
