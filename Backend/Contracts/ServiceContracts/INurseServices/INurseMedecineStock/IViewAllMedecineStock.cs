using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseMedecineStock
{
    public interface IViewAllMedecineStock
    {
        // Returns a list of all active stock DTOs
        Task<List<ViewAllMedecineStockResponseDTO>> ViewAllAsync();
    }
}
