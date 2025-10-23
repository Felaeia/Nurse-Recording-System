using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock
{
    public interface ICreateMedecineStock
    {
        // Returns the ID of the new medicine stock
        Task<int> CreateMedecineStockAsync(CreateMedecineStockRequestDTO request, string createdBy);
    }
}
