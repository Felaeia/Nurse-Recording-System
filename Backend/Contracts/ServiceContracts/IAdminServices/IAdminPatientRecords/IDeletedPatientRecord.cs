namespace NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminPatientRecords
{
    public interface IDeletedPatientRecord
    {
        Task<bool> SoftDeletePatientRecordAsync(int patientRecordId);
    }
}
