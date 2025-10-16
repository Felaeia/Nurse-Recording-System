namespace NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminUser
{
    public interface IDeleteUser
    {
        Task<bool> SoftDeleteUserAsync(int userId, string deletedBy);
    }
}
