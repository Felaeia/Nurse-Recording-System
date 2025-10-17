using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock
{
    public interface IUpdateMedecineStock
    {
        // Returns true if the update was successful
        Task<bool> UpdateAsync(int medicineId, UpdateMedecineStockRequestDTO request, string updatedBy);
    }
}
