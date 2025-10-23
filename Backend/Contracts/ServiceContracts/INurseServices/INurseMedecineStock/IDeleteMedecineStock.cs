namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock
{
    public interface IDeleteMedecineStock
    {
        // Returns true if the soft delete was successful
        Task<bool> DeleteMedecineStockAsync(int medicineId, string deletedBy);
    }
}
