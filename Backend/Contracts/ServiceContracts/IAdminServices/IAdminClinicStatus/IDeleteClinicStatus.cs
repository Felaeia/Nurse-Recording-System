namespace NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus
{
    public interface IDeleteClinicStatus
    {
        Task<bool> DeleteAsync(int logId, string deletedBy);
    }
}
